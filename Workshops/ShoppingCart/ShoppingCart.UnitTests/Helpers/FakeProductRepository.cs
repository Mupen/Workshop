using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.UnitTests.Helpers;

internal sealed class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Product>>(Products);

    public Task<IReadOnlyList<Product>> GetAvailableAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Product>>(_products.Where(product => product.IsAvailable).ToList());

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_products.SingleOrDefault(product => product.Id == id));

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        => Task.FromResult(_products.SingleOrDefault(product => product.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _products.RemoveAll(product => product.Id == id);
        return Task.CompletedTask;
    }
}
