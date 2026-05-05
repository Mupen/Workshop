using ShoppingCart.Application.Interfaces;

namespace ShoppingCart.Application.Queries.ShoppingCarts;

public sealed class GetShoppingCartById
{
    private readonly IShoppingCartRepository _cartRepository;

    public GetShoppingCartById(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    /// <summary>
    /// Gets one shopping cart by id, or returns null if it does not exist.
    /// </summary>
    public Task<ShoppingCart.Domain.Entities.ShoppingCart?> ExecuteAsync(Guid cartId, CancellationToken cancellationToken = default)
        => _cartRepository.GetByIdAsync(cartId, cancellationToken);
}
