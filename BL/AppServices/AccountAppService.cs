using AutoMapper;
using BL.Bases;
using BL.Interfaces;
using BL.Repositories;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Dtos;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BL.AppServices
{
    public class AccountAppService:BaseAppService
    {
        IConfiguration _configuration;

        public AccountAppService(IConfiguration configuration,IUnitOfWork _theUnitOfWork, IMapper _mapper) : base(_theUnitOfWork, _mapper)
        {
            this._configuration = configuration;
            //this.theUnitOfWork = _theUnitOfWork;
            //this.mapper = _mapper;
        }
        public List<ApplicationUserIdentity> GetAll()
        {
            return TheUnitOfWork.Account.GetAllAccounts();
        }

        public async Task<ApplicationUserIdentity> Find(string name, string password)
        {
            ApplicationUserIdentity user = await TheUnitOfWork.Account.Find(name, password);

            if (user != null && user.isDeleted == false)
                return user;
            return null;
        }
        public RegisterViewModel GetAccountById(string id)
        {
            if (id == null)
                throw new ArgumentNullException();
            return Mapper.Map<RegisterViewModel>(TheUnitOfWork.Account.GetAccountById(id));

        }
        public async Task<IdentityResult> Register(RegisterViewModel user)
        {
            bool isExist = await checkUserNameExist(user.UserName);
            if (isExist)
                return IdentityResult.Failed(new IdentityError
                { Code = "error", Description = "user name already exist" });
            ApplicationUserIdentity identityUser = Mapper.Map<RegisterViewModel, ApplicationUserIdentity>(user);
            var result = await TheUnitOfWork.Account.Register(identityUser);
    
            return result;
        }
        public async Task<bool> checkUserNameExist(string userName)
        {
            var user = await TheUnitOfWork.Account.FindByName(userName);
            return user == null ? false : true;
        }
        public async Task<IdentityResult> UpdateUser(RegisterViewModel user)
        {
            ApplicationUserIdentity identityUser = Mapper.Map<RegisterViewModel, ApplicationUserIdentity>(user);
            IdentityResult result = await TheUnitOfWork.Account.UpdateAccount(identityUser);
            return result;
        }


        ///////////////
        ///

        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUserIdentity user)
        {
            return await TheUnitOfWork.Account.GetUserRoles(user);
        }

        public async Task<dynamic> CreateToken(ApplicationUserIdentity user)
        {
            var userRoles = await GetUserRoles(user);
            //var userRoles = new List<String>();
            //userRoles.Add("adad");

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userRoles
            };


        }
    }
}
