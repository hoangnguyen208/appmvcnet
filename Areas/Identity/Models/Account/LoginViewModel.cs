﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Areas.Identity.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Prompt = "Username or Email")]
        public string UserNameOrEmail { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
