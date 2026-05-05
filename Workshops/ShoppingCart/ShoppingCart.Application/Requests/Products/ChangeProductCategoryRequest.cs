namespace ShoppingCart.Application.Requests.Products;

public sealed record ChangeProductCategoryRequest(Guid ProductId, Guid CategoryId);
