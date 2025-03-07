using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPostsAPI.Models;
using UserPostsAPI.DBContext;

public class UsersControllerTests
{
    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_UserNotFound");

        var result = await controller.GetUserById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserExists");
        context.Users.Add(new UserModel
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var result = await controller.GetUserById(1);

        Assert.IsType<ActionResult<UserModel>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("Test User", result.Value.Name);
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenIdIsNegative()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_BadRequest");

        var result = await controller.GetUserById(-1);

        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequest = result.Result as BadRequestObjectResult;
        Assert.Equal("Invalid user ID.", badRequest.Value);
    }

    [Fact]
    public async Task GetUserById_ReturnsInternalServerError_OnException()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_InternalServerError");
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

        var result = await controller.GetUserById(1);

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("Internal server error", statusResult.Value);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var (_, controller) = TestUtils.CreateTestController("TestDatabase_UserNotFound");

        var result = await controller.GetUserPosts(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsNotFound_WhenUserHasNoPosts()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserNoPosts");
        context.Users.Add(new UserModel
        {
            Id = 1,
            Name = "Test Person",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        });
        context.SaveChanges();

        var result = await controller.GetUserPosts(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsPosts_WhenUserExistsAndHasPosts()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_UserWithPosts");
        var user = new UserModel
        {
            Id = 1,
            Name = "Test User",
            Email = "user@example.com",
            Password = "Password123",
            Address = "123 Test Street"
        };
        context.Users.Add(user);
        context.Posts.Add(new UserPostModel
        {
            UserId = user.Id,
            PostContent = "First Post"
        });
        context.SaveChanges();

        var result = await controller.GetUserPosts(1);

        Assert.IsType<ActionResult<IEnumerable<UserPostModel>>>(result);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal("First Post", result.Value.First().PostContent);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsInternalServerError_OnException()
    {
        var (context, controller) = TestUtils.CreateTestController("TestDatabase_InternalServerErrorPosts");
        context.Users.Add(new UserModel
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

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("Internal server error", statusResult.Value);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenDatabaseIsEmpty()
    {
        var (_, controller) = TestUtils.CreateTestController("EmptyTestDatabase");

        var result = await controller.GetUserById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
