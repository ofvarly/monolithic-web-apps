using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts() //to return the status codes, we need to change the return type to IActionResult
        {
            var products = await _context.Products.Select(p => ProductToDTO(p)).ToListAsync();
            return Ok(products);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")] // or [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id) //to return the status codes, we need to change the return type to IActionResult
        {
            if (id == null)
            {
                return NotFound(); // it might be better to return BadRequest() here
            }

            // Retrieve the product entity from the database
            // this didn't work => await _context.Products.Select(p => ProductToDTO(p)).FirstOrDefaultAsync(p => p.ProductId == id);
            var p = await _context.Products.Where(p => p.ProductId == id).Select(p => ProductToDTO(p)).FirstOrDefaultAsync();

            // Check if the product is null
            if (p == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            // Add the new product entity to the Products DbSet
            _context.Products.Add(entity);

            // Save the changes to the database asynchronously
            await _context.SaveChangesAsync();

            // Return a response indicating the product was created successfully
            // The response includes the location of the newly created product
            // CreatedAtAction is used to return a 201 status code and a Location header
            // with the URL of the newly created resource
            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, entity);
        }

        [HttpPut]
        [Route("{id}")] // or [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int? id, Product entity)
        {
            // Check if the id is null
            if (id == null)
            {
                return NotFound();
            }

            // Check if the id in the URL matches the id in the entity
            if (id != entity.ProductId)
            {
                // If the id in the URL does not match the id in the entity, return a 400 Bad Request status code
                // This indicates that the request is invalid
                // The client should correct the request and try again
                return BadRequest();
            }

            // Find the product with the specified id
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            // Check if the product is null
            if (product == null)
            {
                // If the product is null, return a 404 Not Found status code
                // This indicates that the resource was not found
                return NotFound();
            }

            // Update the product properties
            product.ProductName = entity.ProductName;
            product.Price = entity.Price;
            product.IsActive = entity.IsActive;

            // Save the changes to the database asynchronously
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If a DbUpdateConcurrencyException is thrown, return a 404 Not Found status code
                // This indicates that the resource was not found
                return NotFound();
            }

            // Return a 204 No Content status code
            // This indicates that the request was successful and no content is returned
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")] // or [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return NotFound();
            }

            // Find the product with the specified id
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            // Check if the product is null
            if (product == null)
            {
                return NotFound();
            }

            // Remove the product from the Products DbSet
            _context.Products.Remove(product);

            try
            {
                // Save the changes to the database asynchronously
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            // Return a 204 No Content status code
            return NoContent();
        }

        private static ProductDTO ProductToDTO(Product p)
        {
            var entity = new ProductDTO();

            if (p != null)
            {
                entity.ProductId = p.ProductId;
                entity.ProductName = p.ProductName;
                entity.Price = p.Price;
            }

            return entity;
        }
    }
}