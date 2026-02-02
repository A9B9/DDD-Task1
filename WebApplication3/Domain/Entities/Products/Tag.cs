namespace WebApplication3.Domain.Entities.products;

public class Tag : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    // Navigation Properties 
    public ICollection<Product> Products { get; set; } = new List<Product>();
}