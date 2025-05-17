using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc; // Import necessary namespaces for ASP.NET Core MVC
using StoreApp.Data.Abstract; // Import abstract data layer for dependency injection
using StoreApp.Data.Concrete; // Import concrete data layer for dependency injection
using StoreApp.Web.Models; // Import models used in the web application

public class OrderController : Controller // Define OrderController class inheriting from Controller
{
    private Cart cart; // Declare a private field for the cart
    private IOrderRepository _orderRepository; // Declare a private field for the order repository

    // Constructor to initialize cart and order repository
    public OrderController(Cart cartService, IOrderRepository orderRepository)
    {
        cart = cartService; // Initialize cart with the injected cart service
        _orderRepository = orderRepository; // Initialize order repository with the injected repository
    }

    // GET: Displays the checkout page with the current cart
    public IActionResult Checkout()
    {
        return View(new OrderModel() { Cart = cart }); // Return the checkout view with a new OrderModel containing the current cart
    }

    // POST: Processes the checkout form submission
    [HttpPost] // Specify that this action responds to POST requests
    public IActionResult Checkout(OrderModel model)
    {
        // Check if the cart is empty
        if (cart.Items.Count == 0)
        {
            ModelState.AddModelError("", "Sepetinizde ürün yok."); // Add error message if cart is empty
        }

        // Validate the model state
        if (ModelState.IsValid)
        {
            // Create a new order from the model and cart items
            var order = new Order
            {
                Name = model.Name, // Set the order's name from the model
                Email = model.Email, // Set the order's email from the model
                City = model.City, // Set the order's city from the model
                Phone = model.Phone, // Set the order's phone from the model
                AddressLine = model.AddressLine, // Set the order's address line from the model
                OrderDate = DateTime.Now, // Set the order date to the current date and time
                OrderItems = cart.Items.Select(i => new StoreApp.Data.Concrete.OrderItem // Map cart items to order items
                {
                    ProductId = i.Product.Id, // Set the product ID from the cart item
                    Price = (double)i.Product.Price, // Set the price from the cart item
                    Quantity = i.Quantity // Set the quantity from the cart item
                }).ToList() // Convert the result to a list
            };

            model.Cart = cart;
            var payment = ProcessPayment(model);

            // Redirect to the completed order page if payment is successful
            if (payment.Status == "success")
            {
                // Save the order to the repository
                _orderRepository.SaveOrder(order);

                // Clear the cart after order is saved
                cart.Clear();

                return RedirectToPage("/Completed", new { OrderId = order.Id }); // Redirect to the completed page with the order ID (see Completed.cshtml.cs), orderId is added to the route data as query string
            }

             model.Cart = cart;
             return View(model);

        }
        else
        {
            // If model state is invalid, return to the checkout view with the current cart
            model.Cart = cart; // Set the cart in the model
            return View(model); // Return the checkout view with the model
        }
    }

    private Payment ProcessPayment(OrderModel model)
    {
        Options options = new Options();
        options.ApiKey = "sandbox-vHzfGHenDJRtQHBdNr6eAnIVU8EkPh0m";
        options.SecretKey = "sandbox-4XroYMVICHTAoO51qS0rE9RtBkevQgG9";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = new Random().Next(111111111, 999999999).ToString();
        request.Price = model?.Cart?.CalculateTotal().ToString();
        request.PaidPrice = model?.Cart?.CalculateTotal().ToString();
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model?.CartName;
        paymentCard.CardNumber = model?.CartNumber;
        paymentCard.ExpireMonth = model?.ExpirationMonth;
        paymentCard.ExpireYear = model?.ExpirationYear;
        paymentCard.Cvc = model?.Cvc;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = "Johnny";
        buyer.Surname = "Doe";
        buyer.GsmNumber = "+905350000000";
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = "Istanbul";
        buyer.Country = "Turkey";
        buyer.ZipCode = "34732";
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = "Jane Doe";
        shippingAddress.City = "Istanbul";
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        shippingAddress.ZipCode = "34742";
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
        billingAddress.ContactName = "Jane Doe";
        billingAddress.City = "Istanbul";
        billingAddress.Country = "Turkey";
        billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        billingAddress.ZipCode = "34742";
        request.BillingAddress = billingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();

        foreach (var item in model?.Cart?.Items ?? Enumerable.Empty<CartItem>())
        {
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = item.Product.Id.ToString();
            firstBasketItem.Name = item.Product.Name;
            firstBasketItem.Category1 = "Telefon";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = item.Product.Price.ToString();
            basketItems.Add(firstBasketItem);
        }

        request.BasketItems = basketItems;

        Payment payment = Payment.Create(request, options);

        return payment;
    }
}