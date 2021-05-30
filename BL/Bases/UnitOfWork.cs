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

namespace BL.Bases
{
    public class UnitOfWork : IUnitOfWork
    {


        private DbContext EC_DbContext { get; set; }
        private UserManager<ApplicationUserIdentity> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UnitOfWork(ApplicationDBContext EC_DbContext, UserManager<ApplicationUserIdentity> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.EC_DbContext = EC_DbContext;
        }

        public AccountRepositry account;//=> throw new NotImplementedException();
        public AccountRepositry Account
        {
            get
            {
                if (account == null)
                    account = new AccountRepositry(EC_DbContext, _userManager);
                return account;
            }
        }
        public int Commit()
        {
            return EC_DbContext.SaveChanges();
        }

        public void Dispose()
        {
            EC_DbContext.Dispose();
        }
    }
}
