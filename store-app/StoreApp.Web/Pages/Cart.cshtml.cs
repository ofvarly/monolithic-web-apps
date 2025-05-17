using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;

namespace StoreApp.Web.Pages
{
    // The CartModel class handles the operations related to the shopping cart in the web application.
    public class CartModel : PageModel
    {
        private readonly IStoreRepository _storeRepository;

        // Constructor to initialize the store repository.
        public CartModel(IStoreRepository storeRepository, Cart cartService)
        {
            _storeRepository = storeRepository;
            Cart = cartService;
        }

        // Property to hold the Cart object.
        public Cart? Cart { get; set; }

        // OnGet method is called when the page is accessed via a GET request.
        // It retrieves the cart from the session or initializes a new cart if none exists.
        public void OnGet()
        {
        }

        // OnPost method is called when the page is accessed via a POST request.
        // It adds a product to the cart and updates the session.
        public IActionResult OnPost(int Id)
        {
            var product = _storeRepository.Products.FirstOrDefault(p => p.Id == Id);

            // If the product exists, add it to the cart and update the session.
            if (product != null)
            {
                Cart?.AddItem(product, 1);
            }

            // Redirects the user to the home page after adding the product to the cart.
            return RedirectToPage("/cart");
        }

        public IActionResult OnPostRemove(int Id)
        {
            Cart?.RemoveItem(Cart.Items.First(i => i.Product.Id == Id).Product);
            
            return RedirectToPage("/Cart");
        }
    }
}
