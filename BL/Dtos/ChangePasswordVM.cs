﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class ChangePasswordVM
    {
        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Current password")]
        //public string CurrentPassword { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "New password")]
        //public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage =
        //    "The new password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string oldPassword { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string newPassword { get; set; }
    }
}
