using AutoMapper;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;
using MovieReviewApi.Repositories;

namespace MovieReviewApi.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMovieRepository _movieRepository;
        public WatchlistService(IWatchlistRepository watchlistRepository, IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
            _watchlistRepository = watchlistRepository;
        }

        public async Task<Watchlist> GetWatchlistByUserIdAsync(string userId)
        {
            return await _watchlistRepository.GetWatchlistByUserIdAsync(userId) ?? throw new InvalidOperationException("Watchlist not found for given user.");
        }

        public async Task AddToWatchlistAsync(string userId, int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null) throw new InvalidOperationException("Movie not found.");

            await _watchlistRepository.AddToWatchlistAsync(userId, movie);
        }

        public async Task RemoveFromWatchlistAsync(string userId, int movieId)
        {
            var watchlist = await _watchlistRepository.GetWatchlistByUserIdAsync(userId);
            if (watchlist == null) throw new InvalidOperationException("Watchlist not found.");

            await _watchlistRepository.RemoveFromWatchlistAsync(userId, movieId);
        }
    }
}