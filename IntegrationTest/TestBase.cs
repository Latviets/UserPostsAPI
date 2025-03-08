using Microsoft.AspNetCore.Mvc.Testing;
using UserPostsAPI.Data.DBContext;
using UserPostsAPI.Data.Models;

public class TestBase : WebApplicationFactory<Program>
{
    protected readonly HttpClient Client;

    public TestBase()
    {
        // Create an HTTP client to simulate API requests
        Client = CreateClient();
    }

    protected void SeedDatabase(AppDbContext context)
    {
        var users = new List<User>
    {
        new User
        {
            //Id = 1,
            Name = "Edvins",
            Email = "edvins@example.com",
            Password = "Password",
            Address = "Riga, Liela Street 45-26"
        },
        new User
        {
            //Id = 2,
            Name = "Laura",
            Email = "laura@example.com",
            Password = "Password000",
            Address = "Riga, Brivibas Street 12-34"
        }
    };

        context.Users.AddRange(users);

        var posts = new List<UserPost>
    {
        new UserPost
        {
            UserId = 1,
            PostContent = "This is a sample post content."
        }
    };

        context.Posts.AddRange(posts);

        context.SaveChanges();
    }
}
