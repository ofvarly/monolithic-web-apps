using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel 
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "{0} en az {2}, en fazla {1} uzunluğunda olmalı.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

    }
}