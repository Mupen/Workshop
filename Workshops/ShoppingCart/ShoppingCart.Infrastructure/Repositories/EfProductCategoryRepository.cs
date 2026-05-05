using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.Infrastructure.Repositories;

public sealed class EfProductCategoryRepository : IProductCategoryRepository
{
    private readonly ShoppingCartDbContext _dbContext;

    public EfProductCategoryRepository(ShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Reads all product categories from the database ordered by name.
    /// </summary>
    public async Task<IReadOnlyList<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductCategories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds one product category by id, or returns null if it is missing.
    /// </summary>
    public Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProductCategories
            .SingleOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    /// <summary>
    /// Finds one product category by name, or returns null if it is missing.
    /// </summary>
    public Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProductCategories
            .SingleOrDefaultAsync(category => category.Name == name, cancellationToken);
    }

    /// <summary>
    /// Adds a new product category to the database and saves the change.
    /// </summary>
    public async Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        await _dbContext.ProductCategories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves changes made to an existing product category.
    /// </summary>
    public async Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        _dbContext.ProductCategories.Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a product category by id if it exists.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.ProductCategories.SingleOrDefaultAsync(category => category.Id == id, cancellationToken);
        if (category is null)
            return;

        _dbContext.ProductCategories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
