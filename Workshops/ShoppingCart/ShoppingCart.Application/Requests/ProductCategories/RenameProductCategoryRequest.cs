namespace ShoppingCart.Application.Requests.ProductCategories;

public sealed record RenameProductCategoryRequest(Guid CategoryId, string Name);
