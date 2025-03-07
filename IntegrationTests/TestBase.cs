using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using UserPostsAPI.DBContext;
using UserPostsAPI.Models;

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
        context.Users.Add(new UserModel { Id = 1, Name = "Test User" });
        context.SaveChanges();
    }

}
