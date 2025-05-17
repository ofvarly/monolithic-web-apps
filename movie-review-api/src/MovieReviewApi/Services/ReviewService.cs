using AutoMapper;
using MovieReviewApi.Constants;
using MovieReviewApi.DTOs;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;
using MovieReviewApi.Repositories;

namespace MovieReviewApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IMovieRepository _movieRepository;
        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IMovieRepository movieRepository, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _movieRepository = movieRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewReadDto>> GetReviewsByMovieIdAsync(int movieId)
        {
            var reviews = await _reviewRepository.GetReviewsByMovieIdAsync(movieId);
            return _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }

        public async Task AddReviewAsync(ReviewCreateDto reviewDto, int userId)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.UserId = userId;
            await _reviewRepository.AddReviewAsync(review);

            // İlgili Movie'yi al
            var movie = await _movieRepository.GetMovieByIdAsync(reviewDto.MovieId);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found");
            }

            // Movie'nin Ratings listesini dinamik olarak güncelle
            var ratings = await _reviewRepository.GetReviewsByMovieIdAsync(reviewDto.MovieId);
            movie.Ratings = ratings.Select(r => r.Rating).ToList();
            movie.AverageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;

            // Movie'yi güncelle
            await _movieRepository.UpdateMovieAsync(movie);

            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }

        public async Task UpdateReviewAsync(int id, ReviewCreateDto reviewDto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) throw new KeyNotFoundException();

            review.Rating = reviewDto.Rating;
            review.Comment = reviewDto.Comment;

            await _reviewRepository.UpdateReviewAsync(review);

            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            await _reviewRepository.DeleteReviewAsync(review);

            // Cache'i temizle
            _cacheService.Remove(CacheKeys.MoviesList);
        }
    }
}