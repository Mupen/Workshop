namespace ShoppingCart.Application.Requests.ProductCategories;

public sealed record CreateProductCategoryRequest(string Name, Guid? ParentCategoryId = null);
