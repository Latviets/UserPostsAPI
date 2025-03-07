using FluentValidation;
using UserPostsAPI.Models;

public class UserPostModelValidator : AbstractValidator<UserPostModel>
{
    public UserPostModelValidator()
    {
        RuleFor(post => post.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

        RuleFor(post => post.PostContent)
            .NotEmpty()
            .WithMessage("Post content cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Post content cannot exceed 500 characters.");
    }
}
