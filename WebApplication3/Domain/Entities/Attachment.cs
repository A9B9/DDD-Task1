using WebApplication3.Domain.Entities;
using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Domain.Entities;

public class ProductAttachment : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string MetaData { set; get; } = string.Empty;

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}