using FluentValidation;
using UserPostsAPI.Models;

public class UserPostModelTests : TestBase
{
    private readonly UserPostModel _basePost;

    public UserPostModelTests()
    {
        _basePost = new UserPostModel
        {
            Id = 1,
            UserId = 123,
            PostContent = "This is a valid post."
        };
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Invalid()
    {
        var post = _basePost.Clone();
        post.UserId = 0;
        var validator = GetService<IValidator<UserPostModel>>();

        var result = validator.Validate(post);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserId" && e.ErrorMessage.Contains("greater than 0"));
    }

    [Fact]
    public void Should_Pass_When_UserId_Is_Valid()
    {
        var post = _basePost.Clone();
        post.UserId = 2;
        var validator = GetService<IValidator<UserPostModel>>();

        var result = validator.Validate(post);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Have_Error_When_PostContent_Is_Empty()
    {
        var post = _basePost.Clone();
        post.PostContent = string.Empty;
        var validator = GetService<IValidator<UserPostModel>>();

        var result = validator.Validate(post);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PostContent" && e.ErrorMessage.Contains("cannot be empty"));
    }

    [Fact]
    public void Should_Have_Error_When_PostContent_Exceeds_MaxLength()
    {
        var post = _basePost.Clone();
        post.PostContent = new string('A', 501);
        var validator = GetService<IValidator<UserPostModel>>();

        var result = validator.Validate(post);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PostContent" && e.ErrorMessage.Contains("cannot exceed 500 characters"));
    }

    [Fact]
    public void Should_Pass_When_UserPostModel_Is_Valid()
    {
        var post = _basePost.Clone();
        var validator = GetService<IValidator<UserPostModel>>();

        var result = validator.Validate(post);

        Assert.True(result.IsValid);
    }
}
