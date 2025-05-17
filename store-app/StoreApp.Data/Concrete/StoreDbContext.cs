using Microsoft.EntityFrameworkCore;

namespace StoreApp.Data.Concrete
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // creating a many to many relationship between products and categories
            // declaring the list of products in the category model and the list of categories in the product model is already creating the many to many table in database but since we need it to use in the application, we need to use the UsingEntity method and also create the ProductCategory model in Concrete manually 

            modelBuilder.Entity<Product>()
                        .HasMany(e => e.Categories)
                        .WithMany(e => e.Products)
                        .UsingEntity<ProductCategory>();

            // making the category url unique so there cant be more than 1 category with the same url
            modelBuilder.Entity<Category>()
                        .HasIndex(u => u.Url)
                        .IsUnique();

            modelBuilder.Entity<Product>().HasData(
                new List<Product>()
                {
                    new() { Id = 1, Name = "Product1", Description = "Description 1", Price = 100 },
                    new() { Id = 2, Name = "Product2", Description = "Description 2", Price = 200 },
                    new() { Id = 3, Name = "Product3", Description = "Description 3", Price = 300 },
                    new() { Id = 4, Name = "Product4", Description = "Description 4", Price = 400 },
                    new() { Id = 5, Name = "Product5", Description = "Description 5", Price = 500 },
                    new() { Id = 6, Name = "Product6", Description = "Description 6", Price = 600 },
                    new() { Id = 7, Name = "Product7", Description = "Description 7", Price = 700 }
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new() { Id = 1, Name = "Category1", Url = "category1" },
                new() { Id = 2, Name = "Category2", Url = "category2" },
                new() { Id = 3, Name = "Category3", Url = "category3" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory() { ProductId = 1, CategoryId = 1 },
                new ProductCategory() { ProductId = 2, CategoryId = 1 },
                new ProductCategory() { ProductId = 3, CategoryId = 1 },
                new ProductCategory() { ProductId = 4, CategoryId = 2 },
                new ProductCategory() { ProductId = 5, CategoryId = 2 },
                new ProductCategory() { ProductId = 6, CategoryId = 3 },
                new ProductCategory() { ProductId = 7, CategoryId = 3 }
            );
        }
    }
}