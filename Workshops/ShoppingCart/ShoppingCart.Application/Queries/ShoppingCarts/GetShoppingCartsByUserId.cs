using ShoppingCart.Application.Interfaces;

namespace ShoppingCart.Application.Queries.ShoppingCarts;

public sealed class GetShoppingCartsByUserId
{
    private readonly IShoppingCartRepository _cartRepository;

    public GetShoppingCartsByUserId(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    /// <summary>
    /// Gets all shopping carts owned by one user.
    /// </summary>
    public Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
        => _cartRepository.GetByUserIdAsync(userId, cancellationToken);
}
