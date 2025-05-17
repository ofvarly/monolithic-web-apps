using MovieReviewApi.DTOs;

namespace MovieReviewApi.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieReadDto>> GetAllMoviesAsync();
        Task<IEnumerable<MovieReadDto>> GetPopularMoviesAsync();
        Task<IEnumerable<MovieReadDto>> GetMoviesByGenreAsync(string genre);
        Task<MovieReadDto> GetMovieByIdAsync(int id);
        Task<IEnumerable<MovieReadDto>> GetRecentMoviesAsync(int count = 10);
        Task AddMovieAsync(MovieCreateDto movieDto);
        Task UpdateMovieAsync(int id, MovieCreateDto movieDto);
        Task DeleteMovieAsync(int id);
    }
}