﻿using FluentValidation;

namespace UserPostsAPI.Data.Models
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .NotNull()
                .WithMessage("Name is required.")
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