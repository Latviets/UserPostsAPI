using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net.Http.Json;
using UserPostsAPI.Data.DBContext;
using UserPostsAPI.Data.Models;

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

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<User>();
        user.Should().NotBeNull();
        user!.Name.Should().Be("Edvins");
    }


    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var response = await Client.GetAsync("/api/users/999");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenUserIdIsNegative()
    {
        var response = await Client.GetAsync("/api/users/-1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Invalid user ID.");
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserIdIsLarge()
    {
        var response = await Client.GetAsync($"/api/users/{int.MaxValue}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsPosts_WhenUserExistsAndHasPosts()
    {
        var response = await Client.GetAsync("/api/users/1/posts");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var posts = await response.Content.ReadFromJsonAsync<List<UserPost>>();
        posts.Should().NotBeNull();
        posts!.Should().HaveCountGreaterThan(0);
        posts.All(p => p.UserId == 1).Should().BeTrue();
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var response = await Client.GetAsync("/api/users/999/posts");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserIdIsLarge()
    {
        var response = await Client.GetAsync($"/api/users/{int.MaxValue}/posts");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsBadRequest_WhenUserIdIsNegative()
    {
        var response = await Client.GetAsync("/api/users/-1/posts");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Invalid user ID.");
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenDatabaseIsEmpty()
    {
        var controller = DbContextMockHelper.CreateControllerWithMockDbContext(new List<User>(), new List<UserPost>());

        var result = await controller.GetUserPosts(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserExistsButHasNoPosts()
    {
        var mockContext = new Mock<AppDbContext>();

        var users = new List<User>
    {
        new User { Id = 1, Name = "Edvins" },
        new User { Id = 2, Name = "Laura" } // UserId 2 with no posts
    };
        var mockUsersDbSet = DbContextMockHelper.CreateMockDbSet(users);
        mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        var posts = new List<UserPost>(); // No posts for UserId 2
        var mockPostsDbSet = DbContextMockHelper.CreateMockDbSet(posts);
        mockContext.Setup(c => c.Posts).Returns(mockPostsDbSet.Object);

        var controller = new UsersController(mockContext.Object);

        var result = await controller.GetUserPosts(2);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsInternalServerError_WhenExceptionThrown()
    {
        var mockContext = new Mock<AppDbContext>();
        var mockDbSet = new Mock<DbSet<User>>();
        mockDbSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                 .ThrowsAsync(new Exception("Simulated database exception"));

        mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

        var controller = new UsersController(mockContext.Object);

        var result = await controller.GetUserPosts(1);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        statusCodeResult.StatusCode.Should().Be(500);
        statusCodeResult.Value.Should().Be("Internal server error");
    }

    public new void Dispose()
    {
        // Clean up the database
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
