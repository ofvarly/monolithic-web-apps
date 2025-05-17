using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController: ControllerBase
    {
        private readonly IElasticsearchService _elasticsearchService;
        private readonly Serilog.ILogger _logger;

        public SearchController(IElasticsearchService elasticsearchService, Serilog.ILogger logger)
        {
            _elasticsearchService = elasticsearchService;
            _logger = logger;
        }
        
        [HttpPost("index")]
        public async Task<IActionResult> IndexMovie([FromBody] Movie movie)
        {
            await _elasticsearchService.IndexMovieAsync(movie);
            if (movie == null)
            {
                _logger.Warning("Failed to index movie: {Title}", movie!.Title);
                return BadRequest("Failed to index movie.");
            }
            
            _logger.Information("Movie indexed successfully: {Title}", movie.Title);
            return Ok("Movie indexed successfully.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string query)
        {
            var results = await _elasticsearchService.SearchMoviesAsync(query);
            
            if (results == null || !results.Any())
            {
                _logger.Warning("No movies found for query: {Query}", query);
                return NotFound("No movies found.");
            }

            _logger.Information("Search results for query '{Query}': {Results}", query, results);
            return Ok(results);
        }
    }
}