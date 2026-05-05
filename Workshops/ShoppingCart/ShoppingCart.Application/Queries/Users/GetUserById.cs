using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.Users;

public sealed class GetUserById
{
    private readonly IUserRepository _userRepository;

    public GetUserById(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Gets one user by id, or returns null if the account does not exist.
    /// </summary>
    public Task<User?> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
        => _userRepository.GetByIdAsync(userId, cancellationToken);
}
