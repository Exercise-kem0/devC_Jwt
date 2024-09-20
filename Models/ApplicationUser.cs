using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace devC_Jwt.Models
{
    public class ApplicationUser : IdentityUser
    {
        //kk=> to add extra two properties over properties built-in IdentityUser
        [MaxLength(50)]
        public string FirstName { get; set; } 
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
