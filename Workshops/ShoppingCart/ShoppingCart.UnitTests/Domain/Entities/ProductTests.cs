using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.UnitTests.Domain.Entities;

public sealed class ProductTests
{
    private static readonly Guid BrandId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public void Create_Should_create_active_product_with_rounded_price()
    {
        var result = Product.Create(
            "SKU-001",
            "Keyboard",
            "Mechanical keyboard",
            BrandId,
            CategoryId,
            129.995m,
            trackInventory: true,
            stockQuantity: 10);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal("SKU-001", result.Value.Sku);
        Assert.Equal("Keyboard", result.Value.Name);
        Assert.Equal(BrandId, result.Value.BrandId);
        Assert.Equal(CategoryId, result.Value.CategoryId);
        Assert.Equal(130.00m, result.Value.UnitPrice);
        Assert.Equal(10, result.Value.StockQuantity);
        Assert.Equal(ProductStatus.Active, result.Value.Status);
        Assert.True(result.Value.IsAvailable);
    }

    [Fact]
    public void Create_Should_fail_when_name_is_missing()
    {
        var result = Product.Create("SKU-001", "", "Description", BrandId, CategoryId, 100m, true, 10);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InvalidName", result.Error.Code);
    }

    [Fact]
    public void Create_Should_fail_when_price_is_negative()
    {
        var result = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, -1m, true, 10);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InvalidUnitPrice", result.Error.Code);
    }

    [Fact]
    public void Create_Should_allow_untracked_inventory_without_stock_quantity()
    {
        var result = Product.Create(
            "DIGI-001",
            "E-book",
            "Digital download",
            BrandId,
            CategoryId,
            49m,
            trackInventory: false,
            stockQuantity: null);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value.TrackInventory);
        Assert.Null(result.Value.StockQuantity);
        Assert.True(result.Value.IsAvailable);
    }

    [Fact]
    public void Create_Should_fail_when_tracked_inventory_has_no_stock_quantity()
    {
        var result = Product.Create(
            "SKU-001",
            "Keyboard",
            "Description",
            BrandId,
            CategoryId,
            100m,
            trackInventory: true,
            stockQuantity: null);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.StockQuantityRequired", result.Error.Code);
    }

    [Fact]
    public void Create_Should_fail_when_brand_id_is_empty()
    {
        var result = Product.Create("SKU-001", "Keyboard", "Description", Guid.Empty, CategoryId, 100m, true, 10);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InvalidBrandId", result.Error.Code);
    }

    [Fact]
    public void Create_Should_fail_when_category_id_is_empty()
    {
        var result = Product.Create("SKU-001", "Keyboard", "Description", BrandId, Guid.Empty, 100m, true, 10);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InvalidCategoryId", result.Error.Code);
    }

    [Fact]
    public void DecreaseStock_Should_fail_when_quantity_exceeds_stock()
    {
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 2).Value;

        var result = product.DecreaseStock(3);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InsufficientStock", result.Error.Code);
        Assert.Equal(2, product.StockQuantity);
    }

    [Fact]
    public void CanSupply_Should_fail_when_product_is_not_active()
    {
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 10).Value;
        product.ChangeStatus(ProductStatus.Archived);

        var result = product.CanSupply(1);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.NotActive", result.Error.Code);
    }
}
