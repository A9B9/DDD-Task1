using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Domain.Entities.Orders;

public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; private set; }
    public string ProductName { get; private set; }
    public string ProductDescription { get; private set; }
}