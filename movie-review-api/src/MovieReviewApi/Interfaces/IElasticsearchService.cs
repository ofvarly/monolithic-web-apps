using MovieReviewApi.Models;

namespace MovieReviewApi.Interfaces
{
    public interface IElasticsearchService
    {
        Task IndexMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string query);
    }
}