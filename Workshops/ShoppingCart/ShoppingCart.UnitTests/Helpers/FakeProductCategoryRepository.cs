using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.UnitTests.Helpers;

internal sealed class FakeProductCategoryRepository : IProductCategoryRepository
{
    private readonly List<ProductCategory> _categories = [];

    public IReadOnlyList<ProductCategory> Categories => _categories.AsReadOnly();

    public Task<IReadOnlyList<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<ProductCategory>>(Categories);

    public Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_categories.SingleOrDefault(category => category.Id == id));

    public Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => Task.FromResult(_categories.SingleOrDefault(category => category.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        _categories.Add(category);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _categories.RemoveAll(category => category.Id == id);
        return Task.CompletedTask;
    }
}
