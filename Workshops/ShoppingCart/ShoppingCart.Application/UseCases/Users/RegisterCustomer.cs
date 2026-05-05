using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.UseCases.Users;

public sealed class RegisterCustomer
{
    private readonly IUserRepository _userRepository;

    public RegisterCustomer(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Creates a customer account. Customers can shop and manage their own cart.
    /// </summary>
    public async Task<Result<User>> ExecuteAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            return Result<User>.Failure(
                new Error("User.DuplicateEmail", $"User email '{request.Email}' already exists."));
        }

        var result = User.Create(request.Email, request.DisplayName, request.PasswordHash, UserRole.Customer);
        if (result.IsFailure)
            return result;

        await _userRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
