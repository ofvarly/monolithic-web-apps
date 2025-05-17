namespace StoreApp.Data.Concrete;

public class ProductCategory
{
    // creating the many to many table with primary keys, so there wont be same relation in the table
    // for example, product 1 and category 1 can appear only once together
    public int ProductId { get; set; }
    public int CategoryId { get; set; }

}