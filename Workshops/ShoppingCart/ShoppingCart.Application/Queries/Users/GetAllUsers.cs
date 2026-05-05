using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.Users;

public sealed class GetAllUsers
{
    private readonly IUserRepository _userRepository;

    public GetAllUsers(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Gets all user accounts from the repository.
    /// </summary>
    public Task<IReadOnlyList<User>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _userRepository.GetAllAsync(cancellationToken);
}
