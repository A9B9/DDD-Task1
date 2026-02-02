using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Infrastructure.Persistence.Configurations.ProductConfigurations;

public class PriceHistoryConfiguration: IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Price)
            .IsRequired()
            .HasPrecision(3);
        
        builder.HasOne(h => h.Product)
            .WithMany(p => p.PriceHistory)
            .HasForeignKey(h => h.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}