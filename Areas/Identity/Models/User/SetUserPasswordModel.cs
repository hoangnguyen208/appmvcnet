using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using appmvcnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace appmvcnet.Areas.Identity.Models.UserViewModels
{
  public class SetUserPasswordModel
  {
      [Required]
      [DataType(DataType.Password)]
      [Display]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Compare("NewPassword", ErrorMessage = "Passwords did not match.")]
      public string ConfirmPassword { get; set; }


  }
}