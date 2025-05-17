namespace MovieReviewApi.Constants
{
    public static class CacheKeys
    {
        public const string MoviesList = "moviesList";
        public const string MovieById = "movie:{id}";
        public const string PopularMovies = "popularMovies";
        public const string RecentMovies = "recentMovies";
        public const string MovieDetails = "movie:{movieId}:details";
        public const string MoviesByGenre = "genre:{genre}:movies";
        public const string UserWatchlist = "user:{userId}:watchlist";
    }
}