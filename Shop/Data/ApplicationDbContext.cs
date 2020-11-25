using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products  { get; set; }
        public ApplicationDbContext()
        {
            Database.EnsureCreatedAsync();
            Database.MigrateAsync();
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=  Shop.db");
        }
    }

}
