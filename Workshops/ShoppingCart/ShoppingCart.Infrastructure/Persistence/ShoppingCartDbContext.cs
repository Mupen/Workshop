using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Persistence;

public sealed class ShoppingCartDbContext : DbContext
{
    public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gives EF Core access to the Products table.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// Gives EF Core access to the ProductBrands table.
    /// </summary>
    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();

    /// <summary>
    /// Gives EF Core access to the ProductCategories table.
    /// </summary>
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    /// <summary>
    /// Gives EF Core access to the ShoppingCarts table.
    /// </summary>
    public DbSet<ShoppingCart.Domain.Entities.ShoppingCart> ShoppingCarts => Set<ShoppingCart.Domain.Entities.ShoppingCart>();

    /// <summary>
    /// Gives EF Core access to the Users table.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Loads all entity configuration classes so EF Core knows how to map objects to tables.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingCartDbContext).Assembly);
    }
}
