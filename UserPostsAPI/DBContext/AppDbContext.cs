using Microsoft.EntityFrameworkCore;
using UserPostsAPI.Models;

namespace UserPostsAPI.DBContext
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
    }
}
