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
    public PaymentMethod PaymentMethod { get; private set; }
    public bool IsPaid { get; private set; } = false;

    // Shipping 
    public string Country { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;

    #region Domain Methods

    public void AddItem(OrderItem item, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero");
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot add item");
        var orderItem = OrderItems.FirstOrDefault(x => x.Id == item.Id);
        if (orderItem != null)
            orderItem.Quantity += quantity;
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

        var orderItem = OrderItems.FirstOrDefault(x => x.Id == item.Id);
        if (orderItem == null)
            throw new InvalidOperationException("OrderItem not found");
        if (orderItem.Quantity < quantity)
        {
            if (orderItem.Quantity == 0)
            {
                OrderItems.Remove(orderItem);
            }
            orderItem.Quantity -= quantity;
        }
        orderItem.Quantity -= quantity;
    }

    private decimal CalculateSubtotal() =>
      OrderItems.Sum(item => item.Price * item.Quantity);

    private static decimal ApplyDiscount(decimal subtotal, decimal discountRate) =>
        subtotal - (subtotal * discountRate);

    private static decimal ApplyTax(decimal taxRate, decimal discountedSubtotal) =>
         discountedSubtotal - (taxRate * discountedSubtotal);

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new InvalidOperationException("Order is already paid");
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot mark as paid, Order is not confirmed");
        if (TotalCost > 0)
            throw new InvalidOperationException("Order Total cost must be greater than zero");

    }

    public void SetPaymentMethod(PaymentMethod paymentMethod)
    {
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot set payment method for non-confirmed order");
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot set payment method for non-confirmed order");
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
    }

    public void PlaceOrder()
    {
        if (this.OrderStatus != OrderStatus.Draft)
            throw new InvalidOperationException("Order is not active");
        this.OrderStatus = OrderStatus.Placed;
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
        var tax = ApplyTax(taxRate, discounted);
        return subtotal * tax;
    }

    #endregion

}