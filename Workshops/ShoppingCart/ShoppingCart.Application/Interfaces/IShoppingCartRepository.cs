using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Interfaces;

public interface IShoppingCartRepository
{
    Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ShoppingCart.Domain.Entities.ShoppingCart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
