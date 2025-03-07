using Microsoft.AspNetCore.Mvc;
using UserPostsAPI.Models;

public class UsersControllerIntegrationTests : TestBase
{
    private readonly UsersController _controller;

    public UsersControllerIntegrationTests()
    {
        _controller = new UsersController(Context);

        // Seed data
        Context.Users.Add(new UserModel { Id = 1, Name = "Test User" });
        Context.SaveChanges();
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        // Act
        var result = await _controller.GetUserById(1);

        // Assert
        Assert.IsType<ActionResult<UserModel>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal("Test User", result.Value.Name);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Act
        var result = await _controller.GetUserById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsPosts_WhenPostsExist()
    {
        // Arrange
        Context.Posts.Add(new UserPostModel { Id = 1, UserId = 1, PostContent = "Sample Post" });
        Context.SaveChanges();

        // Act
        var result = await _controller.GetUserPosts(1);

        // Assert
        Assert.IsType<ActionResult<IEnumerable<UserPostModel>>>(result);
        Assert.NotEmpty(result.Value);
    }
}
