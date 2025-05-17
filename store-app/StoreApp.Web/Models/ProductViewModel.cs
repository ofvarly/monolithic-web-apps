// Purpose: Contains the ProductViewModel and ProductListViewModel classes. These classes are used to represent the Product model in the web application. The ProductViewModel class is used to represent a single product, while the ProductListViewModel class is used to represent a list of products.

namespace StoreApp.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class ProductListViewModel
    {
        public IEnumerable<ProductViewModel> Products = []; // or Enumerable.Empty<ProductViewModel>() like List = new List<ProductViewModel>();

        public PageInfo PageInfo { get; set; } = new();
    }

    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}