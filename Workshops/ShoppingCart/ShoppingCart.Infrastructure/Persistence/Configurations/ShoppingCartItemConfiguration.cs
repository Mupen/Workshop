using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Persistence.Configurations;

internal sealed class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    /// <summary>
    /// Tells EF Core how the ShoppingCartItem entity should be stored in the database.
    /// </summary>
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.ToTable("ShoppingCartItems");

        builder.HasKey(item => item.Id);

        builder.Property<Guid>("ShoppingCartId")
            .IsRequired();

        builder.Property(item => item.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasPrecision(18, 2);

        builder.HasIndex("ShoppingCartId", nameof(ShoppingCartItem.ProductId))
            .IsUnique();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(item => item.LineTotal);
    }
}
