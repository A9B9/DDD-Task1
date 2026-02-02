using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Domain.Entities.products;

namespace WebApplication3.Infrastructure.Persistence.Configurations.ProductConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        builder.Property(t => t.Description).HasMaxLength(200);
        builder.Property(t => t.Color).IsRequired().HasMaxLength(8);
    }
}