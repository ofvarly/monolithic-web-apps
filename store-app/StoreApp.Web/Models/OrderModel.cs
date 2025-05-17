using System.ComponentModel.DataAnnotations; // Importing necessary namespace for data annotations
using Microsoft.AspNetCore.Mvc.ModelBinding; // Importing necessary namespace for model binding

namespace StoreApp.Web.Models; // Defining the namespace for the OrderModel class

public class OrderModel // Defining the OrderModel class
{
    public int Id { get; set; } // Property to store the order ID
    public DateTime OrderDate { get; set; } // Property to store the date of the order
    public string Name { get; set; } = null!; // Property to store the name of the customer
    public string City { get; set; } = null!; // Property to store the city of the customer
    public string Phone { get; set; } = null!; // Property to store the phone number of the customer

    [EmailAddress] // Data annotation to ensure the email format is valid
    public string Email { get; set; } = null!; // Property to store the email address of the customer
    public string AddressLine { get; set; } = null!; // Property to store the address line of the customer

    [BindNever] // Data annotation to prevent model binding for this property
    public Cart? Cart { get; set; } = null!; // Property to store the cart associated with the order

    public string? CartName { get; set; }
    public string? CartNumber { get; set; }
    public string? ExpirationMonth { get; set; }
    public string? ExpirationYear { get; set; }
    public string? Cvc { get; set; }
}