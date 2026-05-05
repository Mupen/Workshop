namespace ShoppingCart.Application.Requests.Products;

public sealed record UpdateProductDetailsRequest(
    Guid ProductId,
    string Sku,
    string Name,
    string Description);
