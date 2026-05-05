using ShoppingCart.Application.Interfaces;

namespace ShoppingCart.Application.Queries.ShoppingCarts;

public sealed class GetAllShoppingCarts
{
    private readonly IShoppingCartRepository _cartRepository;

    public GetAllShoppingCarts(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    /// <summary>
    /// Gets all shopping carts from the repository.
    /// </summary>
    public Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _cartRepository.GetAllAsync(cancellationToken);
}
