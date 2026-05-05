using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ShoppingCarts;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ShoppingCarts;

public sealed class SetShoppingCartItem
{
    private readonly IShoppingCartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public SetShoppingCartItem(IShoppingCartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Adds, changes, or removes one product item in a shopping cart.
    /// </summary>
    public async Task<Result> ExecuteAsync(SetShoppingCartItemRequest request, CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken);
        if (cart is null)
        {
            return Result.Failure(
                new Error("ShoppingCart.NotFound", "Shopping cart was not found."));
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure(
                new Error("Product.NotFound", "Product was not found."));
        }

        var result = cart.SetItem(product, request.Quantity);
        if (result.IsFailure)
            return result;

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        return Result.Success();
    }
}
