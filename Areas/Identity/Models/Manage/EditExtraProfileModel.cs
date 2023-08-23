using System;
using System.ComponentModel.DataAnnotations;

namespace appmvcnet.Areas.Identity.Models.ManageViewModels
{
  public class EditExtraProfileModel
  {
      public string UserName { get; set; }

      public string UserEmail { get; set; }

      public string PhoneNumber { get; set; }


      [StringLength(400)]
      public string HomeAdress { get; set; }


      public DateTime? BirthDate { get; set; }
  }
}