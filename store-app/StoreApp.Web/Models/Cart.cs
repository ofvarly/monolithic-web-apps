using StoreApp.Data.Concrete;

namespace StoreApp.Web.Models;

public class Cart
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public virtual void AddItem(Product product, int quantity)
    {
        var item = Items.Where(i => i.Product.Id == product.Id).FirstOrDefault();

        // if we add the same product again, we just increase the quantity
        // null means the product is not in the cart, we are adding it for the first time
        if (item == null)
        {
            Items.Add(new CartItem { Product = product, Quantity = quantity });
        }
        else
        {
            item.Quantity += quantity;
        }
    }

    public virtual void RemoveItem(Product product)
    {
        Items.RemoveAll(i => i.Product.Id == product.Id);
    }

    public double CalculateTotal()
    {
        return (double)Items.Sum(i => i.Product.Price * i.Quantity); // i: item
    }

    public virtual void Clear()
    {
        Items.Clear();    
    }
}

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}