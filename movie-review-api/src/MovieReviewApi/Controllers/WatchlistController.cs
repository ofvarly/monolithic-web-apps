using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Interfaces;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;
        private readonly Serilog.ILogger _logger;
        public WatchlistController(IWatchlistService watchlistService, Serilog.ILogger logger)
        {
            _logger = logger;
            _watchlistService = watchlistService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWatchlist(string userId)
        {
            var watchlist = await _watchlistService.GetWatchlistByUserIdAsync(userId);
            if (watchlist == null)
            {
                _logger.Warning("Watchlist not found for user ID {UserId}", userId);
                return NotFound();
            }

            _logger.Information("Fetched watchlist for user ID {UserId}", userId);
            return Ok(watchlist);
        }

        [HttpPost("{userId}/{movieId}")]
        public async Task<IActionResult> AddToWatchlist(string userId, int movieId)
        {
            try
            {
                await _watchlistService.AddToWatchlistAsync(userId, movieId);
                
                _logger.Information("Added movie ID {MovieId} to watchlist for user ID {UserId}", movieId, userId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Error adding movie ID {MovieId} to watchlist for user ID {UserId}", movieId, userId);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}/{movieId}")]
        public async Task<IActionResult> RemoveFromWatchlist(string userId, int movieId)
        {
            try
            {
                await _watchlistService.RemoveFromWatchlistAsync(userId, movieId);
                
                _logger.Information("Removed movie ID {MovieId} from watchlist for user ID {UserId}", movieId, userId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Error removing movie ID {MovieId} from watchlist for user ID {UserId}", movieId, userId);
                return BadRequest(ex.Message);
            }
        }
    }
}