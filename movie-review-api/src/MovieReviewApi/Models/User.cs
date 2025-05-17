using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieReviewApi.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime DateAdded { get; set; }
        public Watchlist? Watchlist { get; set; }
    }
}