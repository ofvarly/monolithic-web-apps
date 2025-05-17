using System.ComponentModel.DataAnnotations;

namespace MovieReviewApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}