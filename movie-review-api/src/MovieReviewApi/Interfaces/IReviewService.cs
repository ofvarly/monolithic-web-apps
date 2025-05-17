using MovieReviewApi.DTOs;

namespace MovieReviewApi.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewReadDto>> GetReviewsByMovieIdAsync(int movieId);
        Task AddReviewAsync(ReviewCreateDto reviewDto, int userId);
        Task UpdateReviewAsync(int id, ReviewCreateDto reviewDto);
        Task DeleteReviewAsync(int id);
    }
}