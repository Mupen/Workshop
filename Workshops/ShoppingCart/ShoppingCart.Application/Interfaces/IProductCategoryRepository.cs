using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Interfaces;

public interface IProductCategoryRepository
{
    Task<IReadOnlyList<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
