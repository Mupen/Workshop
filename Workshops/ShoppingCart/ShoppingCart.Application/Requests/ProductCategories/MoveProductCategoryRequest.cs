namespace ShoppingCart.Application.Requests.ProductCategories;

public sealed record MoveProductCategoryRequest(Guid CategoryId, Guid? ParentCategoryId);
