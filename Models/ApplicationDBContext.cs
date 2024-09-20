using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace devC_Jwt.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser> //if not Customize ur ApplicationUser u must use IdentityUser
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
    }
}
