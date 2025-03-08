using Microsoft.EntityFrameworkCore;
using UserPostsAPI.Data.Models;

namespace UserPostsAPI.Data.DBContext
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) 
        {
        }

        protected AppDbContext() { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPost> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base implementation
            base.OnModelCreating(modelBuilder);

            // Define entity configurations (example below)
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserPost>().ToTable("Posts");
        }
    }
}
