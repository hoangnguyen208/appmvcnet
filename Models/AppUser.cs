using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace appmvcnet.Models
{
    public class AppUser : IdentityUser 
    {
          [Column(TypeName = "nvarchar")]
          [StringLength(400)]  
          public string HomeAdress { get; set; }
  
          [DataType(DataType.Date)]
          public DateTime? BirthDate { get; set; }
    }
}