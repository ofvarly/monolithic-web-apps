using FluentValidation;

namespace MovieReviewApi.DTOs
{
    public class UserReadDto
    {
        public string FullName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
    
        public DateTime DateAdded { get; set; }
    }

    public class UserReadDtoValidator : AbstractValidator<UserReadDto>
    {
        public UserReadDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
            RuleFor(x => x.DateAdded).NotEmpty().WithMessage("Date added is required.");
        }
    }
}