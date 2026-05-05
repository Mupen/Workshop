using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Persistence.Configurations;

internal sealed class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    /// <summary>
    /// Tells EF Core how the ProductBrand entity should be stored in the database.
    /// </summary>
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.ToTable("ProductBrands");

        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(brand => brand.Name)
            .IsUnique();
    }
}
