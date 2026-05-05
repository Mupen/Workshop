using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Tells EF Core how the User entity should be stored in the database.
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(user => user.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasConversion(
                role => role.ToString(),
                value => Enum.Parse<UserRole>(value))
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}
