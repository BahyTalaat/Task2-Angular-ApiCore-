using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class RegisterViewModel
    {
        public String Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

 
        [DataType(DataType.Password)]
        [Required]
        public string PasswordHash { get; set; }

        [Compare("PasswordHash")]
        public string confirmPassword { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public Gender gender { get; set; }
        public string Image { get; set; }
        public bool isDeleted { get; set; }
    }
}
