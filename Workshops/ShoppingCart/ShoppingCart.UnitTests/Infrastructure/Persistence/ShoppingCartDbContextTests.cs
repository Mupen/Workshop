using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.UnitTests.Infrastructure.Persistence;

public sealed class ShoppingCartDbContextTests
{
    [Fact]
    public async Task Database_Should_persist_product_catalog_and_cart_items()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ShoppingCartDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var dbContext = new ShoppingCartDbContext(options))
        {
            await dbContext.Database.EnsureCreatedAsync();

            var brand = ProductBrand.Create("KeyCo").Value;
            var category = ProductCategory.Create("Electronics").Value;
            var product = Product.Create(
                "SKU-001",
                "Keyboard",
                "Description",
                brand.Id,
                category.Id,
                100m,
                trackInventory: true,
                stockQuantity: 10).Value;
            var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
            cart.SetItem(product, 2);

            await dbContext.ProductBrands.AddAsync(brand);
            await dbContext.ProductCategories.AddAsync(category);
            await dbContext.Products.AddAsync(product);
            await dbContext.ShoppingCarts.AddAsync(cart);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ShoppingCartDbContext(options))
        {
            var savedCart = await dbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .SingleAsync();

            Assert.Single(savedCart.Items);
            Assert.Equal(2, savedCart.TotalQuantity);
            Assert.Equal(200m, savedCart.TotalPrice);
        }
    }
}
