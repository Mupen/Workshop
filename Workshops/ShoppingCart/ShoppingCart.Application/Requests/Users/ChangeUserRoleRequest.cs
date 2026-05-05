using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.Requests.Users;

public sealed record ChangeUserRoleRequest(Guid UserId, UserRole Role);
