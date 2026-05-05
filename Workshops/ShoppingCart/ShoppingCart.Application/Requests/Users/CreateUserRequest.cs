using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.Requests.Users;

public sealed record CreateUserRequest(
    string Email,
    string DisplayName,
    string PasswordHash,
    UserRole Role);
