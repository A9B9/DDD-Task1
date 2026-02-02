using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Infrastructure.Persistence.Configurations.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(3);
        
        builder.Property(p => p.Status).IsRequired();
        
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.StatusHistory)
            .WithOne(s => s.Product)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.PriceHistory)
            .WithOne(s => s.Product)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity(j => j.ToTable("ProductTag"));
        
        builder.HasMany(p => p.Attachments)
            .WithOne(a => a.Product)
            .OnDelete(DeleteBehavior.Cascade);
    }
}