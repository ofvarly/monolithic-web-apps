using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {

    }

    public IActionResult Index(string searchString, string category)
    {
        // Retrieve all products from the repository
        var products = Repository.Products;

        // Check if the search string is not null or empty
        if (!String.IsNullOrEmpty(searchString))
        {
            // Store the search string in the ViewBag to retain the value in the view
            ViewBag.SearchString = searchString;

            // Filter the products based on the search string (case insensitive)
            products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
        }

        // Check if the category is not null, empty, or "0" (indicating no category filter)
        if (!String.IsNullOrEmpty(category) && category != "0")
        {
            // Filter the products based on the selected category
            products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        // Create a new ProductViewModel instance to pass to the view
        var model = new ProductViewModel
        {
            // Set the Products property to the filtered list of products
            Products = products,

            // Set the Categories property to the list of all categories from the repository
            Categories = Repository.Categories,

            // Set the SelectedCategory property to the selected category value
            SelectedCategory = category
        };

        // Return the view with the model to display the products and categories
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)
    {
        var extension = "";

        // Check if the uploaded image file is not null
        if (imageFile != null)
        {
            // Define an array of allowed image file extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            // Get the file extension of the uploaded image file
            extension = Path.GetExtension(imageFile.FileName);
            // Check if the uploaded image file contains a valid extension
            if (!allowedExtensions.Contains(extension))
            {
                // Add a model error to the imageFile property with an error message
                ModelState.AddModelError("", "Invalid file type. Only JPG, JPEG, and PNG files are allowed.");
            }
        }

        // Check if the model state is valid
        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                // Generate a random file name using a GUID and append the file extension
                var randomFileName = Guid.NewGuid() + extension;

                // Combine the current directory path with the "wwwroot/img" folder and the random file name to get the full path
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                // Create a new file stream to write the uploaded image file to the specified path
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    // Copy the contents of the uploaded image file to the file stream
                    await imageFile.CopyToAsync(stream);
                }
                // Set the Image property of the model to the random file name
                model.Image = randomFileName;

                // Set the ProductId property of the model to the next available ID
                model.ProductId = Repository.Products.Count + 1;

                // Add the new product to the repository
                Repository.CreateProduct(model);

                // Redirect the user to the Index action
                return RedirectToAction("Index");
            }
        }

        // If the model state is not valid, repopulate the Categories dropdown list
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");

        // Return the view with the model to display validation errors
        return View(model);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);

        if (entity == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
    {
        if (id != model.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                // Get the file extension of the uploaded image file
                var extension = Path.GetExtension(imageFile.FileName);

                // Generate a random file name using a GUID and append the file extension
                var randomFileName = Guid.NewGuid() + extension;

                // Combine the current directory path with the "wwwroot/img" folder and the random file name to get the full path
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                // Create a new file stream to write the uploaded image file to the specified path
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    // Copy the contents of the uploaded image file to the file stream
                    await imageFile.CopyToAsync(stream);
                }

                // Set the Image property of the model to the random file name
                model.Image = randomFileName;
            }

            // Update the product in the repository
            Repository.EditProduct(model);

            // Redirect the user to the Index action
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);

        if (entity == null)
        {
            return NotFound();
        }

        return View("DeleteConfirm", entity);
    }

    [HttpPost]
    public IActionResult Delete(int id, int ProductId)
    {
        if (id != ProductId)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);

        if (entity == null)
        {
            return NotFound();
        }

        Repository.DeleteProduct(entity);

        return RedirectToAction("Index");
    }
}
