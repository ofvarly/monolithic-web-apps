using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Data;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;

namespace MovieReviewApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        {
            return await _context.Reviews
                    .AsNoTracking() // Performans için izlemeyi kapat
                    .Where(r => r.MovieId == movieId)
                    .ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("Review not found.");
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(Review review)
        {
            // Review'ı sil
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // İlgili Movie'yi güncelle
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == review.MovieId);
            if (movie != null)
            {
                var reviewStats = await _context.Reviews
                    .Where(r => r.MovieId == movie.Id)
                    .GroupBy(r => r.MovieId)
                    .Select(g => new
                    {
                        Ratings = g.Count(),
                        AverageRating = g.Average(r => r.Rating)
                    })
                    .FirstOrDefaultAsync();

                movie.Ratings = new List<int> { reviewStats?.Ratings ?? 0 };
                movie.AverageRating = reviewStats?.AverageRating ?? 0;

                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}