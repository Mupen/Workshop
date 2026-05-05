namespace ShoppingCart.Application.Requests.Products;

public sealed record ChangeProductPriceRequest(Guid ProductId, decimal UnitPrice);
