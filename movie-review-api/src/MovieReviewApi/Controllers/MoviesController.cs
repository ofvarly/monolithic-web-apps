using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Constants;
using MovieReviewApi.DTOs;
using MovieReviewApi.Services;

namespace MovieReviewApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly Serilog.ILogger _logger;
        private readonly ICacheService _cacheService;

        public MoviesController(IMovieService movieService, ICacheService cacheService, Serilog.ILogger logger)
        {
            _movieService = movieService;
            _logger = logger.ForContext<MoviesController>();
            _cacheService = cacheService;
        }

        //[Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] PaginationDto paginationDto)
        {
            var cachedMovies = _cacheService.Get<IEnumerable<MovieReadDto>>(CacheKeys.MoviesList);

            IEnumerable<MovieReadDto> movies;

            if (cachedMovies == null)
            {
                _logger.Information(DateTime.Now + " Fetching movies from the database...");
                movies = await _movieService.GetAllMoviesAsync();

                _cacheService.Set(CacheKeys.MoviesList, movies, TimeSpan.FromMinutes(5));

                _logger.Information(DateTime.Now + " Movies cached in Redis with key: {CacheKey}", CacheKeys.MoviesList);
            }
            else
            {
                _logger.Information(DateTime.Now + " Fetching movies from Redis cache...");
                movies = cachedMovies;
            }

            var paginatedMovies = PaginationHelper.Paginate(movies, paginationDto.PageNumber, paginationDto.PageSize);

            _logger.Information(DateTime.Now + " Movies retrieved successfully.");
            return Ok(paginatedMovies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var cachedMovie = _cacheService.Get<MovieReadDto>(CacheKeys.MovieById + id);

            MovieReadDto movie;

            if (cachedMovie == null)
            {
                _logger.Information("Fetching movie with ID {Id} from the database...", id);
                movie = await _movieService.GetMovieByIdAsync(id) ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

                if (movie == null)
                {
                    _logger.Error("Movie not found with ID: {Id}", id);
                    return NotFound();
                }

                _cacheService.Set(CacheKeys.MovieById + id, movie, TimeSpan.FromMinutes(5));
                _logger.Information("Movie cached in Redis with key: {CacheKey}", CacheKeys.MovieById + id);
            }
            else
            {
                _logger.Information("Fetching movie with ID {Id} from Redis cache...", id);
                movie = cachedMovie;
            }

            _logger.Information("Movie retrieved successfully with ID: {Id}", id);
            return Ok(movie);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies([FromQuery] PaginationDto paginationDto)
        {
            var cachedMovies = _cacheService.Get<IEnumerable<MovieReadDto>>(CacheKeys.PopularMovies);

            IEnumerable<MovieReadDto> movies;

            if (cachedMovies == null)
            {
                _logger.Information("Fetching popular movies from the database...");
                movies = await _movieService.GetPopularMoviesAsync();

                _cacheService.Set(CacheKeys.PopularMovies, movies, TimeSpan.FromMinutes(5));
            }
            else
            {
                _logger.Information("Fetching popular movies from Redis cache...");
                movies = cachedMovies;
            }

            var paginatedMovies = PaginationHelper.Paginate(movies, paginationDto.PageNumber, paginationDto.PageSize);

            _logger.Information("Popular movies retrieved successfully.");
            return Ok(paginatedMovies);
        }

        [HttpGet("genre/{genre}")]
        public async Task<IActionResult> GetMoviesByGenre(string genre, [FromQuery] PaginationDto paginationDto)
        {
            var cachedMovies = _cacheService.Get<IEnumerable<MovieReadDto>>(CacheKeys.MoviesByGenre);

            IEnumerable<MovieReadDto> movies;

            if (cachedMovies == null)
            {
                _logger.Information("Fetching movies by genre from the database...");
                movies = await _movieService.GetMoviesByGenreAsync(genre);

                _cacheService.Set(CacheKeys.MoviesByGenre + genre, movies, TimeSpan.FromMinutes(5));
            }
            else
            {
                _logger.Information("Fetching movies by genre from Redis cache...");
                movies = cachedMovies;
            }

            var paginatedMovies = PaginationHelper.Paginate(movies, paginationDto.PageNumber, paginationDto.PageSize);

            _logger.Information("Movies by genre retrieved successfully.");
            return Ok(paginatedMovies);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentMovies([FromQuery] PaginationDto paginationDto)
        {
            var cachedMovies = _cacheService.Get<IEnumerable<MovieReadDto>>(CacheKeys.RecentMovies);

            IEnumerable<MovieReadDto> movies;

            if (cachedMovies == null)
            {
                _logger.Information("Fetching recent movies from the database...");
                movies = await _movieService.GetRecentMoviesAsync();

                _cacheService.Set(CacheKeys.RecentMovies, movies, TimeSpan.FromMinutes(5));
            }
            else
            {
                _logger.Information("Fetching recent movies from Redis cache...");
                movies = cachedMovies;
            }

            var paginatedMovies = PaginationHelper.Paginate(movies, paginationDto.PageNumber, paginationDto.PageSize);

            _logger.Information("Recent movies retrieved successfully.");
            return Ok(paginatedMovies);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieCreateDto movieDto)
        {
            _logger.Information("AddMovie endpoint called with Title: {Title}", movieDto.Title);
            await _movieService.AddMovieAsync(movieDto);

            _logger.Information("Movie added successfully with Title: {Title}", movieDto.Title);
            return CreatedAtAction(nameof(GetMovies), new { id = movieDto.Title }, movieDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieCreateDto movieDto)
        {
            try
            {
                await _movieService.UpdateMovieAsync(id, movieDto);

                _logger.Information("Movie updated successfully with ID: {Id}", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.Error("Movie not found with ID: {Id}", id);
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                _logger.Information("DeleteMovie endpoint called with ID: {Id}", id);
                await _movieService.DeleteMovieAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.Error("Movie not found with ID: {Id}", id);
                return NotFound();
            }
        }

    }
}