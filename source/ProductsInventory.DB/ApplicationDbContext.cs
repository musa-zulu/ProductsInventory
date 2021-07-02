using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductsInventory.DB.Domain;

namespace ProductsInventory.DB
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Category> Categories { get; set; }
        Task<int> SaveChangesAsync();
    }
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Category>().ToTable("Categories");

            builder.Entity<Category>().Property(f => f.Name).IsRequired();
            builder.Entity<Category>().Property(f => f.CategoryCode).IsRequired().HasMaxLength(6);
            builder.Entity<Category>().Property(f => f.IsActive).IsRequired();

            builder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(100);
            builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Entity<User>().Property(u => u.HashedPassword).IsRequired().HasMaxLength(200);
            builder.Entity<User>().Property(u => u.Salt).IsRequired().HasMaxLength(200);
        }
    }
}