using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.ProductCategories;

public sealed record CreateProductCategoryDto(
    [property: Required]
    [property: MaxLength(100)]
    string Name,
    Guid? ParentCategoryId = null);
