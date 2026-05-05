using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Users;

public sealed class ActivateUser
{
    private readonly IUserRepository _userRepository;

    public ActivateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Reactivates a user account so it can be used again.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeUserStatusRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                new Error("User.NotFound", "User was not found."));
        }

        user.Activate();
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
