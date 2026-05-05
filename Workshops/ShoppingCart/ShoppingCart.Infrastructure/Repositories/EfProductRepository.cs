using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.Infrastructure.Repositories;

public sealed class EfProductRepository : IProductRepository
{
    private readonly ShoppingCartDbContext _dbContext;

    public EfProductRepository(ShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Reads all products from the database ordered by name.
    /// </summary>
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Reads products that are active and available to buy.
    /// </summary>
    public async Task<IReadOnlyList<Product>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Status == Domain.Enums.ProductStatus.Active)
            .Where(product => !product.TrackInventory || product.StockQuantity > 0)
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds one product by id, or returns null if it is missing.
    /// </summary>
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .SingleOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    /// <summary>
    /// Finds one product by SKU, or returns null if it is missing.
    /// </summary>
    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .SingleOrDefaultAsync(product => product.Sku == sku, cancellationToken);
    }

    /// <summary>
    /// Adds a new product to the database and saves the change.
    /// </summary>
    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves changes made to an existing product.
    /// </summary>
    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a product by id if it exists.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);
        if (product is null)
            return;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
