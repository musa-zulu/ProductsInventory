using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductsInventory.DB.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProductsInventory.DB
{
    public interface IApplicationDbContext
    {   
        DbSet<Category> Categories { get; set; }
        Task<int> SaveChangesAsync();
    }
    public sealed class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Category> Categories { get; set; }        
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().ToTable("Categories");

            builder.Entity<Category>().Property(f => f.Name).IsRequired();
            builder.Entity<Category>().Property(f => f.CategoryCode).IsRequired().HasMaxLength(6);
            builder.Entity<Category>().Property(f => f.IsActive).IsRequired();
        }
    }
}