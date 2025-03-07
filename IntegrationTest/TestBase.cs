using Microsoft.AspNetCore.Mvc.Testing;
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
        context.Users.Add(new User { Name = "Edvins", Email = "edvins@example.com", Password = "Password123", Address = "Riga, Liela Street 45-26" });
        context.SaveChanges();
    }
}
