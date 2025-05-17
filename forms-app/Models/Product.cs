using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name = "Ürün Id")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Fiyat")]
        public decimal? Price { get; set; }
        
        [Display(Name = "Gorsel")]
        public string? Image { get; set; } = string.Empty;

        [Display(Name="Is Active")]
        public bool isActive { get; set; }

        [Required]
        [Display(Name="Kategori")]
        public int? CategoryId { get; set; }

    }
}