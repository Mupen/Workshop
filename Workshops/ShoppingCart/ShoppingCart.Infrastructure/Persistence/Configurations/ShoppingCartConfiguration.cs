using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCart.Infrastructure.Persistence.Configurations;

internal sealed class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart.Domain.Entities.ShoppingCart>
{
    /// <summary>
    /// Tells EF Core how the ShoppingCart entity should be stored in the database.
    /// </summary>
    public void Configure(EntityTypeBuilder<ShoppingCart.Domain.Entities.ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts");

        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.UserId)
            .IsRequired();

        builder.HasIndex(cart => cart.UserId);

        builder.HasMany(cart => cart.Items)
            .WithOne()
            .HasForeignKey("ShoppingCartId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(cart => cart.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(cart => cart.TotalQuantity);
        builder.Ignore(cart => cart.TotalPrice);
        builder.Ignore(cart => cart.IsEmpty);
    }
}
