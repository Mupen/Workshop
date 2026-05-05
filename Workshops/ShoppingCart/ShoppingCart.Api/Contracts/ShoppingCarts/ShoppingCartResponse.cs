namespace ShoppingCart.Api.Contracts.ShoppingCarts;

public sealed record ShoppingCartResponse(
    Guid Id,
    Guid UserId,
    int TotalQuantity,
    decimal TotalPrice,
    IReadOnlyList<ShoppingCartItemResponse> Items);
