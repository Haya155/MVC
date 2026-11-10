using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaSupply.Models;

namespace UniversityCafeteriaSupply.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<SupplyRequest> SupplyRequests { get; set; }
      



    }
}
