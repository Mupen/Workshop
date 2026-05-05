using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure.Persistence;

namespace ShoppingCart.Infrastructure.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly ShoppingCartDbContext _dbContext;

    public EfUserRepository(ShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Reads all user accounts from the database ordered by email.
    /// </summary>
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.Email)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds one user account by id, or returns null if it is missing.
    /// </summary>
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    /// <summary>
    /// Finds one user account by email, or returns null if it is missing.
    /// </summary>
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return _dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);
    }

    /// <summary>
    /// Adds a new user account to the database and saves the change.
    /// </summary>
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves changes made to an existing user account.
    /// </summary>
    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
