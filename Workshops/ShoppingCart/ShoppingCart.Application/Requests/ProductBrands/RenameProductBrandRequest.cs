namespace ShoppingCart.Application.Requests.ProductBrands;

public sealed record RenameProductBrandRequest(Guid BrandId, string Name);
