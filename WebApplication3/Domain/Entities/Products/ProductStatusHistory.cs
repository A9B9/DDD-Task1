using WebApplication3.Domain.Enums;

namespace WebApplication3.Domain.Entities.products;

public class ProductStatusHistory : EntityBase
{
    public ProductStatus Status { get; set; }

    // Navigation 
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}