using Microsoft.AspNetCore.Identity;

namespace UniversityCafeteriaSupply.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
