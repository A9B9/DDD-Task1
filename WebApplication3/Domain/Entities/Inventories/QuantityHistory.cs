using WebApplication3.Domain.Enums;

namespace WebApplication3.Domain.Entities.Inventories;

public class QuantityHistory : EntityBase
{
    public int Quantity { get; set; }
    public InventoryEvent Event { get; set; }
    public Guid InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}