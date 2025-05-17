using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public int ReleaseYear { get; set; }

        public List<Review>? Reviews { get; set; } = new List<Review>();

        public List<int> Ratings { get; set; } = new List<int>();
        
        public double AverageRating { get; set; } = 0;
    }
}