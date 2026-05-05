using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.Infrastructure.Repositories;

public sealed class EfShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShoppingCartDbContext _dbContext;

    public EfShoppingCartRepository(ShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Reads all shopping carts and their items from the database.
    /// </summary>
    public async Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(cart => cart.Items)
            .OrderBy(cart => cart.UserId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds one shopping cart by id and includes its items.
    /// </summary>
    public Task<ShoppingCart.Domain.Entities.ShoppingCart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.ShoppingCarts
            .Include(cart => cart.Items)
            .SingleOrDefaultAsync(cart => cart.Id == id, cancellationToken);
    }

    /// <summary>
    /// Reads all shopping carts for one user and includes their items.
    /// </summary>
    public async Task<IReadOnlyList<ShoppingCart.Domain.Entities.ShoppingCart>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(cart => cart.Items)
            .Where(cart => cart.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new shopping cart to the database and saves the change.
    /// </summary>
    public async Task AddAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        await _dbContext.ShoppingCarts.AddAsync(cart, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves changes made to an existing shopping cart.
    /// </summary>
    public async Task UpdateAsync(ShoppingCart.Domain.Entities.ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        _dbContext.ShoppingCarts.Update(cart);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a shopping cart by id if it exists.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _dbContext.ShoppingCarts.SingleOrDefaultAsync(cart => cart.Id == id, cancellationToken);
        if (cart is null)
            return;

        _dbContext.ShoppingCarts.Remove(cart);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
