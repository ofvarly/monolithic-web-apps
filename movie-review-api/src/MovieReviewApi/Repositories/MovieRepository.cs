using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Data;
using MovieReviewApi.Models;

namespace MovieReviewApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;
        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.AsNoTracking().ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id) ??
                    throw new KeyNotFoundException("Movie not found");
        }
        
        public async Task<IEnumerable<Movie>> GetPopularMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .OrderByDescending(m => m.AverageRating)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Genre != null && m.Genre.Contains(genre))
                .OrderBy(m => m.Title) // Başlığa göre sıralama
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetRecentMoviesAsync(int count = 10)
        {
            return await _context.Movies
                .AsNoTracking()
                .OrderByDescending(m => m.ReleaseYear)
                .Take(count)
                .ToListAsync();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

    }
}