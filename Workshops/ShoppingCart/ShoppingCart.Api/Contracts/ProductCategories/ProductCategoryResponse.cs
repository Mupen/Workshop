namespace ShoppingCart.Api.Contracts.ProductCategories;

public sealed record ProductCategoryResponse(
    Guid Id,
    string Name,
    Guid? ParentCategoryId,
    bool IsRootCategory);
