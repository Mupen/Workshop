using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.ProductBrands;

public sealed record RenameProductBrandDto(
    [property: Required]
    [property: MaxLength(100)]
    string Name);
