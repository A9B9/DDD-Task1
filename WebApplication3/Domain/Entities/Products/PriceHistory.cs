namespace WebApplication3.Domain.Entities.products;

public class PriceHistory : EntityBase
{
    public decimal Price { get; set; }

    // Navigation 
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}