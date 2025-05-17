using FluentValidation;

namespace MovieReviewApi.DTOs
{
    public class MovieCreateDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public int ReleaseYear { get; set; }
    }
    
    public class MovieCreateDtoValidator : AbstractValidator<MovieCreateDto>
    {
        public MovieCreateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Genre).NotEmpty().WithMessage("Genre is required.");
            RuleFor(x => x.ReleaseYear).InclusiveBetween(1900, DateTime.Now.Year)
                .WithMessage($"ReleaseYear must be between 1900 and {DateTime.Now.Year}.");
            RuleFor(x => x.Description).MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.Title).MaximumLength(100)
                .WithMessage("Title cannot exceed 100 characters.");
        }
    }
}
