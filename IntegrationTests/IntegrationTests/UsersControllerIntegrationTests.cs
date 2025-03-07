using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UserPostsAPI.DBContext;
using UserPostsAPI.Models;


public class UsersControllerIntegrationTests : TestBase, IDisposable
{
    private readonly AppDbContext _context;

    public UsersControllerIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        SeedDatabase(_context);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var response = await Client.GetAsync("/api/users/1");

        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserModel>();
        Assert.NotNull(user);
        Assert.Equal("Test User", user.Name);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var response = await Client.GetAsync("/api/users/999");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenUserIdIsNegative()
    {
        var response = await Client.GetAsync("/api/users/-1");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid user ID.", content);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserIdIsLarge()
    {
        var response = await Client.GetAsync($"/api/users/{int.MaxValue}");


        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_HandlesConcurrentRequests_Correctly()
    {
        var tasks = new[]
        {
            Client.GetAsync("/api/users/1"),
            Client.GetAsync("/api/users/1")
        };

        var responses = await Task.WhenAll(tasks);

        foreach (var response in responses)
        {
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            Assert.NotNull(user);
            Assert.Equal("Test User", user.Name);
        }
    }

    public void Dispose()
    {
        // Clean up the database
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
