
namespace FormsApp.Models
{
    public class Repository
    {
        private static readonly List<Product> _products = new();
        private static readonly List<Category> _categories = new();

        static Repository()
        {
            _categories.Add(new Category { CategoryId = 1, Name = "Telefon" });
            _categories.Add(new Category { CategoryId = 2, Name = "Bilgisayar" });

            _products.Add(new Product { CategoryId = 1, Image = "1.jpg", isActive = true, Name = "iPhone 12", Price = 40000, ProductId = 1 });
            _products.Add(new Product { CategoryId = 1, Image = "2.jpg", isActive = true, Name = "iPhone 13", Price = 50000, ProductId = 2 });
            _products.Add(new Product { CategoryId = 1, Image = "3.jpg", isActive = true, Name = "iPhone 14", Price = 60000, ProductId = 3 });
            _products.Add(new Product { CategoryId = 1, Image = "4.jpg", isActive = true, Name = "iPhone 15", Price = 70000, ProductId = 4 });
            _products.Add(new Product { CategoryId = 2, Image = "5.jpg", isActive = true, Name = "Macbook Air", Price = 80000, ProductId = 5 });
            _products.Add(new Product { CategoryId = 2, Image = "6.jpg", isActive = true, Name = "Macbook Pro", Price = 90000, ProductId = 6 });
        }

        public static void CreateProduct(Product entity)
        {
            _products.Add(entity);
        }

        public static void EditProduct(Product updatedProduct)
        {
            var entity = _products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);

            if (entity != null)
            {
                entity.Name = updatedProduct.Name;
                entity.Image = updatedProduct.Image;
                entity.Price = updatedProduct.Price;
                entity.CategoryId = updatedProduct.CategoryId;
                entity.isActive = updatedProduct.isActive;
            }
        }

        public static void DeleteProduct(Product deletedProduct)
        {
            var entity = _products.FirstOrDefault(p => p.ProductId == deletedProduct.ProductId);

            if (entity != null)
            {
                _products.Remove(entity);
            }
        }

        public static List<Product> Products { get { return _products; } }

        public static List<Category> Categories { get { return _categories; } }
    }
}