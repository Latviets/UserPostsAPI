using FluentValidation;

namespace UserPostsAPI.Models
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Name must consist of letters only.")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters long.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required.");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");

            RuleFor(user => user.Address)
                .NotEmpty()
                .WithMessage("Address is required.");
        }
    }
}
