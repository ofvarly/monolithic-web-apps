using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace MovieReviewApi.DTOs
{
    public class ReviewCreateDto
    {
        public int MovieId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating is required.");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
            RuleFor(x => x.MovieId).NotEmpty().WithMessage("MovieId is required.");
            RuleFor(x => x.Rating).InclusiveBetween(1, 10)
                .WithMessage("Rating must be between 1 and 10.");
            RuleFor(x => x.Comment).MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.");
            RuleFor(x => x.MovieId).GreaterThan(0)
                .WithMessage("MovieId must be greater than 0.");
        }
    }
}