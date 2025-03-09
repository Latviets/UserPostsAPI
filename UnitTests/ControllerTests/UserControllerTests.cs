using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UserPostsAPI.Data.Models;

public class UsersControllerTests
{
    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_UserNotFound");

        var result = await controller.GetUserById(999);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserExists");
        context.Users.Add(new User
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var result = await controller.GetUserById(1);

        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(1);
        result.Value.Name.Should().Be("Test User");
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenIdIsNegative()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_BadRequest");

        var result = await controller.GetUserById(-1);

        result.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Invalid user ID.");
    }

    [Fact]
    public async Task GetUserById_ReturnsInternalServerError_OnException()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_InternalServerError");
        context.Users.Add(new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        // Simulate failure by disposing of the context
        context.Dispose();

        var result = await controller.GetUserById(1);

        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(500);
        result.Result.As<ObjectResult>().Value.Should().Be("Internal server error");
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_UserNotFound");

        var result = await controller.GetUserPosts(999);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserHasNoPosts()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserNoPosts");
        context.Users.Add(new User
        {
            Id = 1,
            Name = "Test Person",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var result = await controller.GetUserPosts(1);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetUserPosts_ReturnsPosts_WhenUserExistsAndHasPosts()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserWithPosts");
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        };
        context.Users.Add(user);
        context.Posts.Add(new UserPost
        {
            UserId = user.Id,
            PostContent = "First Post"
        });
        context.SaveChanges();

        var result = await controller.GetUserPosts(1);

        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(1);
        result.Value.First().PostContent.Should().Be("First Post");
    }

    [Fact]
    public async Task GetUserPosts_ReturnsInternalServerError_OnException()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_InternalServerErrorPosts");
        context.Users.Add(new User
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        // Simulate failure by disposing of the context
        context.Dispose();

        var result = await controller.GetUserPosts(1);

        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(500);
        result.Result.As<ObjectResult>().Value.Should().Be("Internal server error");
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenDatabaseIsEmpty()
    {
        var (_, controller) = TestUtils.CreateTestController("EmptyTestDatabase");

        var result = await controller.GetUserById(1);

        result.Result.Should().BeOfType<NotFoundResult>();
    }
}