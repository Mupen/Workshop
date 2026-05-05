using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Users;

public sealed class ChangeUserRole
{
    private readonly IUserRepository _userRepository;

    public ChangeUserRole(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Changes a user's role if the user account exists.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeUserRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                new Error("User.NotFound", "User was not found."));
        }

        var result = user.ChangeRole(request.Role);
        if (result.IsFailure)
            return result;

        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
