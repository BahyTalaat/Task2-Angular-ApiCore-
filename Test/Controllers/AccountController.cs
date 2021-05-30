
using BL.AppServices;
using BL.Dtos;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountAppService acount;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        IHttpContextAccessor _httpContextAccessor;
        public AccountController(AccountAppService acount, UserManager<ApplicationUserIdentity> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.acount = acount;
            this._userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await acount.Register(user);

                if (!result.Succeeded)
                    return BadRequest((result.Errors.ToList())[0]);

                //ApplicationUserIdentity identityUser = await acount.Find(user.UserName, user.password);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LogViewModel useVM)
        {
           //var PasswordHash = _userManager.PasswordHasher.HashPassword(useVM.passwordHash);
            var user = await acount.Find(useVM.UserName, useVM.PasswordHash);
            if (user != null)
            {
                dynamic token = await acount.CreateToken(user);
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id,RegisterViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = acount.UpdateUser(user);
                return Ok("Updated");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
           var userID =  _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res =  acount.GetAccountById(userID);

            return Ok(res);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
            if (!result.Succeeded)
            {
                return Ok("Password cant change");
            }
            return Ok(result);
        }

    }
}
