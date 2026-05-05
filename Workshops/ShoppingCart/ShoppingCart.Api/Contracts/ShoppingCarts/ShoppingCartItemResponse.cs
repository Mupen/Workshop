namespace ShoppingCart.Api.Contracts.ShoppingCarts;

public sealed record ShoppingCartItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
