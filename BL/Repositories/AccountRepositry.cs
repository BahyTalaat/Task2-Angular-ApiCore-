using BL.Bases;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class AccountRepositry : BaseRepositroy<ApplicationUserIdentity>
    {
        private readonly UserManager<ApplicationUserIdentity> manager;
        public AccountRepositry(DbContext db, UserManager<ApplicationUserIdentity> manager) : base(db)
        {
            this.manager = manager;

        }
        public ApplicationUserIdentity GetAccountById(string id)
        {
            return GetFirstOrDefault(l => l.Id == id);
        }
        public List<ApplicationUserIdentity> GetAllAccounts()
        {
            return GetAll().ToList();
        }
        public async Task<ApplicationUserIdentity> FindByName(string userName)
        {
            ApplicationUserIdentity result = await manager.FindByNameAsync(userName);
            return result;
        }
        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUserIdentity user)
        {
            var userRoles = await manager.GetRolesAsync(user);
            return userRoles;
        }
        public async Task<ApplicationUserIdentity> Find(string username, string password)
        {
            var user = await manager.FindByNameAsync(username);
            if (user != null && await manager.CheckPasswordAsync(user, password))
            {
                return user;
            }

            return null;

        }
        public async Task<IdentityResult> Register(ApplicationUserIdentity user)
        {
            user.Id = Guid.NewGuid().ToString();
            IdentityResult result;
            result = await manager.CreateAsync(user, user.PasswordHash);

            return result;
        }
        public async Task<bool> updatePassword(ApplicationUserIdentity user)
        {
            manager.PasswordHasher.HashPassword(user, user.PasswordHash);
            IdentityResult result = await manager.UpdateAsync(user);
            return true;
        }
        public async Task<IdentityResult> UpdateAccount(ApplicationUserIdentity user)
        {
            IdentityResult result = await manager.UpdateAsync(user);
            return result;  
        }
    }
}
