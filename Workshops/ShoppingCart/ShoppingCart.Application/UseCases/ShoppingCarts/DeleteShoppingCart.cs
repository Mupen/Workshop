using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ShoppingCarts;

public sealed class DeleteShoppingCart
{
    private readonly IShoppingCartRepository _cartRepository;

    public DeleteShoppingCart(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    /// <summary>
    /// Deletes a shopping cart if it exists.
    /// </summary>
    public async Task<Result> ExecuteAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId, cancellationToken);
        if (cart is null)
        {
            return Result.Failure(
                new Error("ShoppingCart.NotFound", "Shopping cart was not found."));
        }

        await _cartRepository.DeleteAsync(cartId, cancellationToken);
        return Result.Success();
    }
}
