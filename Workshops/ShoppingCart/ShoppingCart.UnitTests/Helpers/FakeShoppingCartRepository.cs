using ShoppingCart.Application.Interfaces;

namespace ShoppingCart.UnitTests.Helpers;

internal sealed class FakeShoppingCartRepository : IShoppingCartRepository
{
    private readonly List<ShoppingCart.Domain.Entities.ShoppingCart> _carts = [];

    public IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart> Carts => _carts.AsReadOnly();

    public Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>>(Carts);

    public Task<ShoppingCart.Domain.Entities.ShoppingCart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_carts.SingleOrDefault(cart => cart.Id == id));

    public Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>>(_carts.Where(cart => cart.UserId == userId).ToList());

    public Task AddAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        _carts.Add(cart);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _carts.RemoveAll(cart => cart.Id == id);
        return Task.CompletedTask;
    }
}
