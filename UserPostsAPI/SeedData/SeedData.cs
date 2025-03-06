using Microsoft.EntityFrameworkCore;
using UserPostsAPI.DBContext;
using UserPostsAPI.Models;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new List<UserModel>
            {
                new UserModel
                {
                    Name = "Edvins",
                    Email = "edvins@example.com",
                    Password = "Password123",
                    Address = "Riga, Liela Street 45-26"
                },
                new UserModel
                {
                    Name = "Liga",
                    Email = "liga@example.com",
                    Password = "Password456",
                    Address = "Ventspils, Liela Street 45-26"
                },
                new UserModel
                {
                    Name = "Laura",
                    Email = "laura@example.com",
                    Password = "Password789",
                    Address = "Tukums, Liela Street 45-26"
                },
                new UserModel
                {
                    Name = "Roberts",
                    Email = "roberts@example.com",
                    Password = "Password321",
                    Address = "Talsi, Liela Street 45-26"
                }
            };

            context.Users.AddRange(users);

            context.SaveChanges();

            var userPosts = new Dictionary<string, List<UserPostModel>>
            {
                { "Edvins", new List<UserPostModel>
                    {
                        new UserPostModel { PostContent = "My first post" },
                        new UserPostModel { PostContent = "My second post" }
                    }
                },
                { "Liga", new List<UserPostModel>
                    {
                        new UserPostModel { PostContent = "Liga's first post" }
                    }
                },
                { "Laura", new List<UserPostModel>
                    {
                        new UserPostModel { PostContent = "Laura's only post" }
                    }
                },
                { "Roberts", new List<UserPostModel>
                    {
                        new UserPostModel { PostContent = "Robert's first post" },
                        new UserPostModel { PostContent = "Robert's second post" },
                        new UserPostModel { PostContent = "Robert's third post" }
                    }
                }
            };

            foreach (var user in users)
            {
                var posts = userPosts[user.Name];
                foreach (var post in posts)
                {
                    post.UserId = user.Id;
                }
                context.Posts.AddRange(posts);
            }

            context.SaveChanges();
        }
    }
}
