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

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserPostModel> Posts { get; set; }
    }
}
