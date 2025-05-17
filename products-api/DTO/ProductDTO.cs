namespace ProductsAPI.DTO
{
    // DTO is an abbreviation for Data Transfer Object
    // DTOs are used to transfer data between software application subsystems
    // they are like ViewModels, we take only the necessary (what we want to get specifically) from the original model
    // in this case, we don't want to return the IsActive status from the Model
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }        
    }
}