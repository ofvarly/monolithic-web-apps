using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReviewApi.Models;

namespace MovieReviewApi.Interfaces
{
    public interface IWatchlistService
    {
        Task<Watchlist> GetWatchlistByUserIdAsync(string userId);
        Task AddToWatchlistAsync(string userId, int movieId);
        Task RemoveFromWatchlistAsync(string userId, int movieId);
        
    }
}