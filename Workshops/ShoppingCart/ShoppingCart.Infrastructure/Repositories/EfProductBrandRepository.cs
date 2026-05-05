using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.Infrastructure.Repositories;

public sealed class EfProductBrandRepository : IProductBrandRepository
{
    private readonly ShoppingCartDbContext _dbContext;

    public EfProductBrandRepository(ShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Reads all product brands from the database ordered by name.
    /// </summary>
    public async Task<IReadOnlyList<ProductBrand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductBrands
            .AsNoTracking()
            .OrderBy(brand => brand.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds one product brand by id, or returns null if it is missing.
    /// </summary>
    public Task<ProductBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProductBrands
            .SingleOrDefaultAsync(brand => brand.Id == id, cancellationToken);
    }

    /// <summary>
    /// Finds one product brand by name, or returns null if it is missing.
    /// </summary>
    public Task<ProductBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.ProductBrands
            .SingleOrDefaultAsync(brand => brand.Name == name, cancellationToken);
    }

    /// <summary>
    /// Adds a new product brand to the database and saves the change.
    /// </summary>
    public async Task AddAsync(ProductBrand brand, CancellationToken cancellationToken = default)
    {
        await _dbContext.ProductBrands.AddAsync(brand, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves changes made to an existing product brand.
    /// </summary>
    public async Task UpdateAsync(ProductBrand brand, CancellationToken cancellationToken = default)
    {
        _dbContext.ProductBrands.Update(brand);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a product brand by id if it exists.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await _dbContext.ProductBrands.SingleOrDefaultAsync(brand => brand.Id == id, cancellationToken);
        if (brand is null)
            return;

        _dbContext.ProductBrands.Remove(brand);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
