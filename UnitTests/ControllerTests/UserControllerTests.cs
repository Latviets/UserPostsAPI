using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPostsAPI.Models;
using UserPostsAPI.DBContext;
using Moq;
using Microsoft.VisualStudio.CodeCoverage;

public class UsersControllerTests
{
    private readonly AppDbContext _context;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        // Configure the in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        // Seed the database with test data
        SeedDatabase();

        // Initialize the controller with the in-memory database context
        _controller = new UsersController(_context);
    }

    private void SeedDatabase()
    {
        //Id = 1,
        _context.Users.Add(new UserModel
        {          
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });

        //Id = 2,
        _context.Users.Add(new UserModel
        {   
            Name = "Another User",
            Email = "another@example.com",
            Password = "Password456",
            Address = "456 Another Avenue"
        });

        _context.SaveChanges();
    }


    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var result = await _controller.GetUserById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var result = await _controller.GetUserById(1); // First user added in SeedDataBase

        Assert.IsType<ActionResult<UserModel>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("Test User", result.Value.Name);
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenIdIsNegative()
    {
        var result = await _controller.GetUserById(-1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequest = result.Result as BadRequestObjectResult;
        Assert.Equal("Invalid user ID.", badRequest.Value);
    }

    [Fact]
    public async Task GetUserById_ReturnsInternalServerError_OnException()
    {
        // Test-specific DbContext with in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_InternalServerError")
            .Options;

        using var context = new AppDbContext(options);

        context.Users.Add(new UserModel
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        // Simulate failure by disposing of the context before the controller can use it
        context.Dispose();

        var controller = new UsersController(context);

        var result = await controller.GetUserById(1);

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("Internal server error", statusResult.Value);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_UserNotFound")
            .Options;

        using var context = new AppDbContext(options);
        var controller = new UsersController(context);

        // Act
        var result = await controller.GetUserPosts(999); // Nonexistent user ID

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }


    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserHasNoPosts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_UserNoPosts")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.Add(new UserModel
        {
            Name = "Test Person",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var controller = new UsersController(context);

        // Act
        var result = await controller.GetUserPosts(1); // User exists but has no posts

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsPosts_WhenUserExistsAndHasPosts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_UserWithPosts")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.Add(new UserModel
        {
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });

        context.Posts.Add(new UserPostModel
        {
            PostContent = "First Post"
        });
        context.SaveChanges();

        var controller = new UsersController(context);

        // Act
        var result = await controller.GetUserPosts(1);

        // Assert
        Assert.IsType<ActionResult<IEnumerable<UserPostModel>>>(result);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal("First Post", result.ToString());
    }

    [Fact]
    public async Task GetUserPosts_ReturnsInternalServerError_OnException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_InternalServerErrorPosts")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.Add(new UserModel
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var controller = new UsersController(context);

        // Simulate failure by disposing of the context
        context.Dispose();

        // Act
        var result = await controller.GetUserPosts(1);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("Internal server error", statusResult.Value);
    }


}
