using Microsoft.EntityFrameworkCore;
using UserPostsAPI.DBContext;
using UserPostsAPI.Models;

public class SeedData
{
    private static readonly object _lock = new object();

    public static void Initialize(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>();

        lock (_lock)
        {
            try
            {
                using var context = new AppDbContext(options);

                if (context.Users.Any())
                {
                    return;
                }

                var users = new List<User>
                {
                    new User { Name = "Edvins", Email = "edvins@example.com", Password = "Password123", Address = "Riga, Liela Street 45-26" },
                    new User { Name = "Liga", Email = "liga@example.com", Password = "Password456", Address = "Ventspils, Liela Street 45-26" },
                    new User { Name = "Laura", Email = "laura@example.com", Password = "Password789", Address = "Tukums, Liela Street 45-26" },
                    new User { Name = "Roberts", Email = "roberts@example.com", Password = "Password321", Address = "Talsi, Liela Street 45-26" }
                };

                foreach (var user in users)
                {
                    if (!context.Users.Any(u => u.Email == user.Email))
                    {
                        context.Users.Add(user);
                    }
                }

                context.SaveChanges();

                var userPosts = new Dictionary<string, List<UserPost>>
                {
                    { "Edvins", new List<UserPost>
                        {
                            new UserPost { PostContent = "My first post" },
                            new UserPost { PostContent = "My second post" }
                        }
                    },
                    { "Liga", new List<UserPost>
                        {
                            new UserPost { PostContent = "Liga's first post" }
                        }
                    },
                    { "Laura", new List<UserPost>
                        {
                            new UserPost { PostContent = "Laura's only post" }
                        }
                    },
                    { "Roberts", new List<UserPost>
                        {
                            new UserPost { PostContent = "Robert's first post" },
                            new UserPost { PostContent = "Robert's second post" },
                            new UserPost { PostContent = "Robert's third post" }
                        }
                    }
                };

                foreach (var user in users)
                {
                    var existingUser = context.Users.Single(u => u.Email == user.Email);
                    var posts = userPosts[existingUser.Name];
                    foreach (var post in posts)
                    {
                        post.UserId = existingUser.Id;
                    }
                    context.Posts.AddRange(posts);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during database seeding: {ex.Message}");
                throw new InvalidOperationException("Database seeding failed", ex);
            }
        }
    }
}
