using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.Orders;

namespace WebApplication3.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).IsRequired();
        builder.Property(o => o.OrderStatus).IsRequired();
        
        builder.Property(o => o.Discount).IsRequired();
        builder.Property(o => o.Subtotal).IsRequired();
        builder.Property(o => o.TotalCost).IsRequired();
        builder.Property(o => o.Tax).IsRequired();
        
        builder.Property(o => o.Country).IsRequired();
        builder.Property(o => o.City).IsRequired();
        builder.Property(o => o.Street).IsRequired();
        
        builder.Property(o => o.IsPaid).IsRequired();
        builder.Property(o => o.PaymentMethod).IsRequired();
        
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        

    }
}