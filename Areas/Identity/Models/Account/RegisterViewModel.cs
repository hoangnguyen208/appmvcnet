// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Areas.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Prompt = "Email")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords did not match.")]
        [Display(Prompt = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.Text)]
        [Required]
        [Display(Prompt = "UserName")]
        public string UserName { get; set; }

    }
}
