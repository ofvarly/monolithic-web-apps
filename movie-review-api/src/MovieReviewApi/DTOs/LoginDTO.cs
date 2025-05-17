using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace MovieReviewApi.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
