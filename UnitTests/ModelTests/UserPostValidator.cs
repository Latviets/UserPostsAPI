using FluentValidation;
using UserPostsAPI.Models;

public class UserPostValidator : AbstractValidator<UserPost>
{
    public UserPostValidator()
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
