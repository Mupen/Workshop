using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.Users;

public sealed class GetUserByEmail
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmail(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Gets one user by email, or returns null if no account uses that email.
    /// </summary>
    public Task<User?> ExecuteAsync(string email, CancellationToken cancellationToken = default)
        => _userRepository.GetByEmailAsync(email, cancellationToken);
}
