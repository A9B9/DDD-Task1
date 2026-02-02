using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.Orders;

namespace WebApplication3.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).IsRequired();
        builder.Property(i => i.ProductDescription)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.Price)
            .IsRequired()
            .HasPrecision(3);
        
        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Order)
            .WithMany(i => i.OrderItems)
            .HasForeignKey(i => i.OrderId);
    }
}