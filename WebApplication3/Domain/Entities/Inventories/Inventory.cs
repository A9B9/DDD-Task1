using WebApplication3.Domain.Entities.products;
using WebApplication3.Domain.Enums;

namespace WebApplication3.Domain.Entities.Inventories;

public class Inventory : EntityBase
{
    public int StockAvailable { get; private set; }
    public int StockReserved { get; private set; }

    // LifeCycle 
    public bool IsLocked { get; private set; }
    public bool IsSellable { get; private set; }

    // Navigation 
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public ICollection<QuantityHistory> QuantityHistory { get; private set; } = [];

    #region Domain Methods
    
    public int TotalStock() => StockAvailable + StockReserved;
    
    public void Reserve(int quantity)
    {
        if(quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),"Quantity must be greater than zero");
        if(IsLocked)
            throw new InvalidOperationException("Inventory is Locked");
        if(quantity > StockAvailable)
            throw new InvalidOperationException(
                "Stock reserve cannot be greater than the stock available");
        StockAvailable -= quantity;
        StockReserved += quantity;
        var qtyHistory = new QuantityHistory()
        {
            Quantity = quantity,
            Inventory = this,
            Event = InventoryEvent.StockReserved,
        };
        QuantityHistory.Add(qtyHistory);
        
    }

    public void Release(int quantity)
    {
        if(quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),"Quantity must be greater than zero");
        if(IsLocked)
            throw new InvalidOperationException("Inventory is Locked");
        if(quantity > StockReserved)
            throw new InvalidOperationException(
                "Stock release cannot be greater than the stock available");
        StockAvailable += quantity;
        StockReserved -= quantity;
        var qtyHistory = new QuantityHistory()
        {
            Quantity = quantity,
            Inventory = this,
            Event = InventoryEvent.StockReleased,
        };
        QuantityHistory.Add(qtyHistory);
    }

    public void IncreaseStock(int quantity)
    {
        if(quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),"Quantity must be greater than zero");
        if( IsLocked )
            throw new InvalidOperationException("Inventory is locked");
        StockAvailable += quantity;
        var qtyHistory = new QuantityHistory()
        {
            Quantity = quantity,
            Inventory = this,
            Event = InventoryEvent.StockReceived,
        };
        QuantityHistory.Add(qtyHistory);
    }

    public void Sell(int quantity)
    {
        if(quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),"Quantity must be greater than zero");
        if(IsLocked)
            throw new InvalidOperationException("Inventory is locked");
        if(quantity > StockAvailable)
            throw new InvalidOperationException(
                "Stock decrease cannot be greater than the stock available");
        StockAvailable -= quantity;
        var qtyHistory = new QuantityHistory()
        {
            Quantity = quantity,
            Inventory = this,
            Event = InventoryEvent.StockSold,
        };
        QuantityHistory.Add(qtyHistory);
    }
    
    public void Adjust(int quantity)
    {
        if(quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),"Quantity must be greater than zero");
        if(IsLocked)
            throw new InvalidOperationException("Inventory is locked");
        if(quantity > StockAvailable)
            throw new InvalidOperationException(
                "Stock decrease cannot be greater than the stock available");
        StockAvailable -= quantity;
        var qtyHistory = new QuantityHistory()
        {
            Quantity = quantity,
            Inventory = this,
            Event = InventoryEvent.StockAdjusted,
        };
        QuantityHistory.Add(qtyHistory);
    }

    public void Lock() => IsLocked = true;

    public void Unlock() => IsLocked = false; 

    public void MarkSellable()
    {
        if(StockAvailable == 0)
            throw new InvalidOperationException("Inventory is Empty");
        if(IsLocked)
            throw new InvalidOperationException("Inventory is locked");
        IsSellable = true;
    }

    public void UnmarkSellable()
    {
        if(IsLocked)
            throw new InvalidOperationException("Inventory is locked");
        IsSellable = false;
    }

    #endregion
}