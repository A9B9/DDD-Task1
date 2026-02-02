using WebApplication3.Domain.Enums;

namespace WebApplication3.Domain.Entities.Orders;

public class Order : EntityBase
{
    // Order info
    public string OrderNumber { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = [];

    // Cost && Payment
    public decimal? Discount { get; private set; }
    public decimal? Tax { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal TotalCost { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod.None;
    public bool IsPaid { get; private set; } = false;
    public bool PriceLocked { get; private set; } = false;

    // Shipping 
    public string Country { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;

    #region Domain Methods

    public void AddItem(OrderItem item, int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero");
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot add item");
        var orderItem = OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (orderItem != null)
        {
            orderItem.IncreaseQuantity(quantity);
            return;
        }
        item.IncreaseQuantity(quantity);
        OrderItems.Add(item);
    }

    public void RemoveItem(OrderItem item, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero");
        if (OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot remove item");
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");

        var orderItem = OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId) ??
         throw new InvalidOperationException("OrderItem not found");

        if (quantity > orderItem.Quantity)
            throw new InvalidOperationException("Quantity to remove exceeds item quantity");

        if (orderItem.Quantity <= quantity)
        {
            OrderItems.Remove(orderItem);
            return;
        }
        orderItem.DecreaseQuantity(quantity);
    }

    private decimal CalculateSubtotal() =>
      OrderItems.Sum(item => item.Price * item.Quantity);

    private static decimal ApplyDiscount(decimal subtotal, decimal discountRate) =>
        subtotal - (subtotal * discountRate);

    private static decimal ApplyTax(decimal taxRate, decimal discountedSubtotal) =>
         discountedSubtotal * taxRate;

    public void MarkAsPaid()
    {
        if (!PriceLocked)
            throw new InvalidOperationException("Order price is locked");
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot mark as paid, Order is not confirmed");
        if (TotalCost <= 0)
            throw new InvalidOperationException("Order Total cost must be greater than zero");
        IsPaid = true;
    }

    public void SetPaymentMethod(PaymentMethod paymentMethod)
    {
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot set payment method for non-confirmed order");
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (paymentMethod == PaymentMethod.None)
            throw new InvalidOperationException("Invalid payment method");
        PaymentMethod = paymentMethod;
    }

    public void SetShippingAddress(string country, string city, string street)
    {
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot set shipping");
        Country = country;
        City = city;
        Street = street;
    } // fixed 

    public void PlaceOrder(decimal taxRate, decimal discountRate)
    {
        if (OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Only Draft Orders can be placed");
        decimal subtotal = CalculateSubtotal();
        decimal Total = CalculateTotalCost(taxRate, discountRate);
        if (string.IsNullOrWhiteSpace(Country) ||
            string.IsNullOrWhiteSpace(City) ||
            string.IsNullOrWhiteSpace(Street))
            throw new InvalidOperationException("Shipping address is not set");

        if (OrderItems.Count == 0)
            throw new InvalidOperationException("Order must have at least one item");
        if (Total <= 0)
            throw new InvalidOperationException("Order Total cost must be greater than zero");

        TotalCost = Total;
        Subtotal = subtotal;
        Discount = discountRate;
        Tax = taxRate;
        OrderStatus = OrderStatus.Placed;
        PriceLocked = true;
    }

    public void ConfirmOrder()
    {
        if (OrderStatus != OrderStatus.Placed)
            throw new InvalidOperationException("Order is not placed");
        OrderStatus = OrderStatus.Confirmed;
    }

    public void CancelOrder()
    {
        OrderStatus = OrderStatus switch
        {
            OrderStatus.Confirmed or OrderStatus.Draft or OrderStatus.Placed
                => OrderStatus.Cancelled,
            _ => throw new InvalidOperationException("Only Confirmed, Draft or Placed Orders are supported")
        };
    }

    public void ShipOrder()
    {
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Order is not confirmed");
        if (PaymentMethod == PaymentMethod.Online && !IsPaid)
            throw new InvalidOperationException("Payment is not completed");
        OrderStatus = OrderStatus.Shipped;
    }

    public void DeleteOrder()
    {
        OrderStatus = OrderStatus switch
        {
            OrderStatus.Draft or OrderStatus.Cancelled or OrderStatus.Rejected => OrderStatus.Deleted,
            _ => throw new InvalidOperationException("only canceled, draft, or rejected orders can be deleted"),
        };
    }
    public decimal CalculateTotalCost(decimal taxRate, decimal discountRate)
    {
        var subtotal = CalculateSubtotal();
        var discounted = ApplyDiscount(subtotal, discountRate);
        var taxed = ApplyTax(taxRate, discounted);
        return taxed;
    }

    #endregion

}