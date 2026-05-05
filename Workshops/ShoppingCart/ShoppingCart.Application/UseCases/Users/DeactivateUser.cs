using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Users;

public sealed class DeactivateUser
{
    private readonly IUserRepository _userRepository;

    public DeactivateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Deactivates a user account so it can no longer be used.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeUserStatusRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                new Error("User.NotFound", "User was not found."));
        }

        user.Deactivate();
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
