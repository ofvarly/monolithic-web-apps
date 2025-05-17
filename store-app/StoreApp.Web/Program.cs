using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Concrete;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// added mapper to the dependency injection container so we can use it in project
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// added dbcontext to the dependency injection container so we can use it in project
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("StoreApp.Web"))
);

// added abstract and concrete interface to the dependency injection container so we can use it in project
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

// Add distributed memory cache to the dependency injection container
builder.Services.AddDistributedMemoryCache();

// Add session services to the dependency injection container
builder.Services.AddSession();

// Add singleton service for IHttpContextAccessor to the dependency injection container
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add scoped service for Cart to the dependency injection container, using SessionCart to get the cart
builder.Services.AddScoped<Cart>(sc => SessionCart.GetCart(sc));

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();
app.MapControllerRoute("products_in_category", "products/{category?}", new { controller = "Home", action = "Index" });

app.MapControllerRoute("product_details", "{name}", new { controller = "Home", action = "Details" });

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
