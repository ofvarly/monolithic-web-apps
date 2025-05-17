using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReviewApi.Models;

namespace MovieReviewApi.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<Watchlist?> GetWatchlistByUserIdAsync(string userId);
        Task AddToWatchlistAsync(string userId, Movie movie);
        Task RemoveFromWatchlistAsync(string userId, int movieId);
    }
}