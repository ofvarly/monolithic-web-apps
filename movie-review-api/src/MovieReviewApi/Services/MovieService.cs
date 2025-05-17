using AutoMapper;
using MovieReviewApi.Constants;
using MovieReviewApi.DTOs;
using MovieReviewApi.Models;
using MovieReviewApi.Repositories;

namespace MovieReviewApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IMapper _mapper;

        private readonly ICacheService _cacheService;

        public MovieService(IMovieRepository repository, IMapper mapper, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _repository = repository;
            _mapper = mapper;
        }
    
        public async Task<IEnumerable<MovieReadDto>> GetAllMoviesAsync()
        {
            var movies = await _repository.GetAllMoviesAsync();
            return _mapper.Map<IEnumerable<MovieReadDto>>(movies);
        }

        public async Task<MovieReadDto> GetMovieByIdAsync(int id)
        {
            var movie = await _repository.GetMovieByIdAsync(id) ?? throw new KeyNotFoundException("Movie not found");
            return _mapper.Map<MovieReadDto>(movie);
        }

        public async Task<IEnumerable<MovieReadDto>> GetPopularMoviesAsync()
        {
            var movies = await _repository.GetPopularMoviesAsync();
            return _mapper.Map<IEnumerable<MovieReadDto>>(movies);
        }

        public async Task<IEnumerable<MovieReadDto>> GetMoviesByGenreAsync(string genre)
        {
            var movies = await _repository.GetMoviesByGenreAsync(genre);
            return _mapper.Map<IEnumerable<MovieReadDto>>(movies);
        }

        public async Task<IEnumerable<MovieReadDto>> GetRecentMoviesAsync(int count = 10)
        {
            var movies = await _repository.GetRecentMoviesAsync(count);
            return _mapper.Map<IEnumerable<MovieReadDto>>(movies);
        }
        
        public async Task AddMovieAsync(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            await _repository.AddMovieAsync(movie);

            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }

        public async Task UpdateMovieAsync(int id, MovieCreateDto movieDto)
        {
            var movie = await _repository.GetMovieByIdAsync(id);

            _mapper.Map(movieDto, movie);
            await _repository.UpdateMovieAsync(movie);
            
            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _repository.GetMovieByIdAsync(id);

            await _repository.DeleteMovieAsync(movie);

            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }

        
    }
}