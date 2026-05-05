using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.UnitTests.Helpers;

internal sealed class FakeProductBrandRepository : IProductBrandRepository
{
    private readonly List<ProductBrand> _brands = [];

    public IReadOnlyList<ProductBrand> Brands => _brands.AsReadOnly();

    public Task<IReadOnlyList<ProductBrand>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<ProductBrand>>(Brands);

    public Task<ProductBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_brands.SingleOrDefault(brand => brand.Id == id));

    public Task<ProductBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => Task.FromResult(_brands.SingleOrDefault(brand => brand.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(ProductBrand brand, CancellationToken cancellationToken = default)
    {
        _brands.Add(brand);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ProductBrand brand, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _brands.RemoveAll(brand => brand.Id == id);
        return Task.CompletedTask;
    }
}
