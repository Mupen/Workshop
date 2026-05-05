using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.UseCases.Users;

public sealed class CreateUser
{
    private readonly IUserRepository _userRepository;

    public CreateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Creates a user account with the requested role if the email is not already used.
    /// </summary>
    public async Task<Result<User>> ExecuteAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            return Result<User>.Failure(
                new Error("User.DuplicateEmail", $"User email '{request.Email}' already exists."));
        }

        var result = User.Create(request.Email, request.DisplayName, request.PasswordHash, request.Role);
        if (result.IsFailure)
            return result;

        await _userRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
