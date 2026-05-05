using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.UnitTests.Domain.Entities;

public sealed class ShoppingCartTests
{
    private static readonly Guid BrandId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public void Create_Should_fail_when_user_id_is_empty()
    {
        var result = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal("ShoppingCart.InvalidUserId", result.Error.Code);
    }

    [Fact]
    public void SetItem_Should_add_product_item_and_calculate_totals()
    {
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 99.995m, true, 10).Value;

        var result = cart.SetItem(product, 2);

        Assert.True(result.IsSuccess);
        Assert.Single(cart.Items);
        Assert.Equal(2, cart.TotalQuantity);
        Assert.Equal(200.00m, cart.TotalPrice);
    }

    [Fact]
    public void SetItem_Should_update_existing_product_item()
    {
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 10).Value;
        cart.SetItem(product, 2);

        var result = cart.SetItem(product, 4);

        Assert.True(result.IsSuccess);
        Assert.Single(cart.Items);
        Assert.Equal(4, cart.TotalQuantity);
        Assert.Equal(400m, cart.TotalPrice);
    }

    [Fact]
    public void SetItem_Should_remove_existing_product_item_when_quantity_is_zero()
    {
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 10).Value;
        cart.SetItem(product, 2);

        var result = cart.SetItem(product, 0);

        Assert.True(result.IsSuccess);
        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
    }

    [Fact]
    public void SetItem_Should_fail_when_quantity_exceeds_stock()
    {
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 1).Value;

        var result = cart.SetItem(product, 2);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.InsufficientStock", result.Error.Code);
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void SetItem_Should_fail_when_product_is_not_active()
    {
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 10).Value;
        product.ChangeStatus(ProductStatus.Archived);

        var result = cart.SetItem(product, 1);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.NotActive", result.Error.Code);
        Assert.Empty(cart.Items);
    }
}
