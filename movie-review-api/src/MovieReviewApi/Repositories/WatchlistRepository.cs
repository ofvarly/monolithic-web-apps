using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Data;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;

namespace MovieReviewApi.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly AppDbContext _context;
        public WatchlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Watchlist?> GetWatchlistByUserIdAsync(string userId)
        {
            var watchlist = await _context.Watchlists
                .AsNoTracking()
                .Include(w => w.Movies)
                .FirstOrDefaultAsync(w => w.UserId == userId) ?? 
                new Watchlist
                {
                    UserId = userId,
                    Movies = new List<Movie>()
                };

            return watchlist;
        }

        public async Task AddToWatchlistAsync(string userId, Movie movie)
        {
            var watchlist = await GetWatchlistByUserIdAsync(userId);
            watchlist!.Movies.Add(movie);

            _context.Watchlists.Update(watchlist);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWatchlistAsync(string userId, int movieId)
        {
            var watchlist = await GetWatchlistByUserIdAsync(userId);

            var movie = watchlist!.Movies.FirstOrDefault(m => m.Id == movieId);

            if (movie != null)
            {
                watchlist.Movies.Remove(movie);
                _context.Watchlists.Update(watchlist);
                await _context.SaveChangesAsync();
            }
        }
    }
}