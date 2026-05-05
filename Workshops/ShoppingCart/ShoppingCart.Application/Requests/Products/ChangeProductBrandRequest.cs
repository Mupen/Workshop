namespace ShoppingCart.Application.Requests.Products;

public sealed record ChangeProductBrandRequest(Guid ProductId, Guid BrandId);
