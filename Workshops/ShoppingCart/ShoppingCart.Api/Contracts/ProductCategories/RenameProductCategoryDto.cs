using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.ProductCategories;

public sealed record RenameProductCategoryDto(
    [property: Required]
    [property: MaxLength(100)]
    string Name);
