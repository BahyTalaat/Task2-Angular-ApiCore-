using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ApplicationUserIdentity : IdentityUser
    {
        [Required]
        public string Address { get; set; }
        public string Country { get; set; }
        [Required]
        public Gender gender { get; set; }
        public string Image { get; set; }
        public bool isDeleted { get; set; }
    }
    public enum Gender{
        Male,
        Female
    }
    public class ApplicationUserStore : UserStore<ApplicationUserIdentity>
    {

        public ApplicationUserStore() : base(new ApplicationDBContext())
        {

        }
        public ApplicationUserStore(DbContext db) : base(db)
        {

        }
    }
    public class ApplicationDBContext : IdentityDbContext<ApplicationUserIdentity>
    {
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .UseSqlServer("Data Source=.;Initial Catalog=TaskDB;Integrated Security=True"
               , options => options.EnableRetryOnFailure());
        }


    }

}
