using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ShoppingCarts;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ShoppingCarts;

public sealed class CreateShoppingCart
{
    private readonly IShoppingCartRepository _cartRepository;

    public CreateShoppingCart(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    /// <summary>
    /// Creates an empty shopping cart for the requested user.
    /// </summary>
    public async Task<Result<ShoppingCart.Domain.Entities.ShoppingCart>> ExecuteAsync(CreateShoppingCartRequest request, CancellationToken cancellationToken = default)
    {
        var result = ShoppingCart.Domain.Entities.ShoppingCart.Create(request.UserId);
        if (result.IsFailure)
            return result;

        await _cartRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
