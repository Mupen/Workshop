namespace ShoppingCart.Application.Requests.ShoppingCarts;

public sealed record SetShoppingCartItemRequest(Guid CartId, Guid ProductId, int Quantity);
