using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Interfaces;

public interface IProductBrandRepository
{
    Task<IReadOnlyList<ProductBrand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(ProductBrand brand, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductBrand brand, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
