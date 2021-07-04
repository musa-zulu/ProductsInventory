using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductsInventory.DB.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProductsInventory.DB
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        Task<int> SaveChangesAsync();
    }
    public sealed class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {     
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Product>().ToTable("Products");

            builder.Entity<Category>().Property(f => f.CategoryId).IsRequired();
            builder.Entity<Category>().Property(f => f.Name).IsRequired();
            builder.Entity<Category>().Property(f => f.CategoryCode).IsRequired().HasMaxLength(6);
            builder.Entity<Category>().Property(f => f.IsActive).IsRequired();

            builder.Entity<Product>().Property(f => f.ProductId).IsRequired();
            builder.Entity<Product>().Property(f => f.Name).IsRequired();
            builder.Entity<Product>().Property(f => f.ProductCode).IsRequired().HasMaxLength(10);          
            builder.Entity<Product>().Property(f => f.Price).IsRequired();
            builder.Entity<Product>().HasOne(e => e.Category);
        }
    }
}