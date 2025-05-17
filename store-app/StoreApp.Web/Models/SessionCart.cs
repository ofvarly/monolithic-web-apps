using System.Text.Json.Serialization;
using StoreApp.Data.Concrete;
using StoreApp.Web.TagHelpers;

namespace StoreApp.Web.Models;

public class SessionCart : Cart
{
    // Retrieves the cart from the session or creates a new one if it doesn't exist
    public static Cart GetCart(IServiceProvider services)
    {
        // Get the session from the HTTP context
        ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        // Try to get the cart from the session, if not found create a new SessionCart
        SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
        // Assign the session to the cart
        cart.Session = session;
        return cart;
    }

    // Ignore this property during JSON serialization
    [JsonIgnore]
    public ISession? Session { get; set; }

    // Adds an item to the cart and updates the session
    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        // Save the updated cart to the session
        Session?.SetJson("Cart", this);
    }

    // Removes an item from the cart and updates the session
    public override void RemoveItem(Product product)
    {
        base.RemoveItem(product);
        // Save the updated cart to the session
        Session?.SetJson("Cart", this);
    }

    // Clears the cart and removes it from the session
    public override void Clear()
    {
        base.Clear();
        // Remove the cart from the session
        Session?.Remove("Cart");
    }
}