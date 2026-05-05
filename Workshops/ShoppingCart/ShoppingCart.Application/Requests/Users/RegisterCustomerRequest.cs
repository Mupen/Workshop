namespace ShoppingCart.Application.Requests.Users;

public sealed record RegisterCustomerRequest(
    string Email,
    string DisplayName,
    string PasswordHash);
