using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class ECommerceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ECommerceDbContext():base()
        {
            
        }
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Category> Categories => Set<Category>();
    }
}
