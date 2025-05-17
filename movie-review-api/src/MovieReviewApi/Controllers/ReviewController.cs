using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.DTOs;
using MovieReviewApi.Interfaces;

namespace MovieReviewApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly Serilog.ILogger _logger;

        public ReviewsController(IReviewService reviewService, Serilog.ILogger logger)
        {
            _reviewService = reviewService;
            _logger = logger.ForContext<ReviewsController>();
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetReviews(int movieId)
        {
            var reviews = await _reviewService.GetReviewsByMovieIdAsync(movieId);

            if (reviews == null || !reviews.Any())
            {
                return NotFound(new { Message = "No reviews found for this movie." });
            }

            _logger.Information(DateTime.Now + " Reviews retrieved successfully for movie ID: {MovieId}", movieId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto reviewDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            int userId = 1;
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }
            
            // _logger.Error("User ID not found in token."); 
            // return Unauthorized(new { Message = "User ID not found in token." });

            await _reviewService.AddReviewAsync(reviewDto, userId);

            _logger.Information("Review added successfully for movie ID: {MovieId} by user ID: {UserId}", reviewDto.MovieId, userId);
            return CreatedAtAction(nameof(GetReviews), new { movieId = reviewDto.MovieId }, reviewDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewCreateDto reviewDto)
        {
            await _reviewService.UpdateReviewAsync(id, reviewDto);

            _logger.Information("Review updated successfully for review ID: {ReviewId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);

                _logger.Information("Review deleted successfully for review ID: {ReviewId}", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.Error("Review not found with ID: {ReviewId}", id);
                return NotFound();
            }
        }
    }
}