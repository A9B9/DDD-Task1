using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.Inventories;
namespace WebApplication3.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class InventoryConfiguration:IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.StockAvailable).IsRequired();
        builder.Property(i => i.StockReserved).IsRequired();
        builder.Property(i => i.IsLocked).IsRequired();
        builder.Property(i => i.IsSellable).IsRequired();
        
        builder.HasOne(i => i.Product)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(i => i.QuantityHistory)
            .WithOne(h => h.Inventory)
            .OnDelete(DeleteBehavior.Cascade);
    }
}