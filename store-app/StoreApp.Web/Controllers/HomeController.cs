using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;

namespace StoreApp.Web.Controllers;

public class HomeController : Controller
{
    public int pageSize = 3;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;

    public HomeController(IStoreRepository storeRepository, IMapper mapper)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
    }

    

    // Action method with a query string parameter for pagination
    public IActionResult Index(string category, int page = 1)
    {
        // the reason we are using ProductListViewModel is because we can add more properties to it in the future
        // since we require a ProductListViewModel as model to the view, we are creating an instance of it and passing the products to it
        return View(new ProductListViewModel
        {
            Products = _storeRepository.
                        GetProductsByCategory(category, page, pageSize).
                        Select(p => _mapper.Map<ProductViewModel>(p)),
            PageInfo = new PageInfo
            {
                TotalItems = _storeRepository.GetProductCount(category),
                ItemsPerPage = pageSize,
                CurrentPage = page
            }
        });
    }
}
