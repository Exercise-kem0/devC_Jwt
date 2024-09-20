using System.ComponentModel.DataAnnotations;

namespace devC_Jwt.Models
{
    public class RegisterModel
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; } //extra two addded properties in ApplicationUser.cs
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string Password { get; set; }
    }
}
