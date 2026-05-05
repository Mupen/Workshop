using ShoppingCart.Application.Requests.ShoppingCarts;
using ShoppingCart.Application.UseCases.ShoppingCarts;
using ShoppingCart.Domain.Entities;
using ShoppingCart.UnitTests.Helpers;

namespace ShoppingCart.UnitTests.Application.UseCases;

public sealed class SetShoppingCartItemTests
{
    private static readonly Guid BrandId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public async Task ExecuteAsync_Should_add_product_to_cart()
    {
        var carts = new FakeShoppingCartRepository();
        var products = new FakeProductRepository();
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        var product = Product.Create("SKU-001", "Keyboard", "Description", BrandId, CategoryId, 100m, true, 10).Value;
        await carts.AddAsync(cart);
        await products.AddAsync(product);
        var useCase = new SetShoppingCartItem(carts, products);

        var result = await useCase.ExecuteAsync(new SetShoppingCartItemRequest(cart.Id, product.Id, 2));

        Assert.True(result.IsSuccess);
        Assert.Single(cart.Items);
        Assert.Equal(2, cart.TotalQuantity);
    }

    [Fact]
    public async Task ExecuteAsync_Should_fail_when_cart_does_not_exist()
    {
        var carts = new FakeShoppingCartRepository();
        var products = new FakeProductRepository();
        var useCase = new SetShoppingCartItem(carts, products);

        var result = await useCase.ExecuteAsync(new SetShoppingCartItemRequest(Guid.NewGuid(), Guid.NewGuid(), 1));

        Assert.True(result.IsFailure);
        Assert.Equal("ShoppingCart.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task ExecuteAsync_Should_fail_when_product_does_not_exist()
    {
        var carts = new FakeShoppingCartRepository();
        var products = new FakeProductRepository();
        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(Guid.NewGuid()).Value;
        await carts.AddAsync(cart);
        var useCase = new SetShoppingCartItem(carts, products);

        var result = await useCase.ExecuteAsync(new SetShoppingCartItemRequest(cart.Id, Guid.NewGuid(), 1));

        Assert.True(result.IsFailure);
        Assert.Equal("Product.NotFound", result.Error.Code);
    }
}
