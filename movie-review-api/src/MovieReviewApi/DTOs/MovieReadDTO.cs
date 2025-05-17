namespace MovieReviewApi.DTOs
{
    public class MovieReadDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int ReleaseYear { get; set; }
        public double AverageRating { get; set; } = 0;
    }
}