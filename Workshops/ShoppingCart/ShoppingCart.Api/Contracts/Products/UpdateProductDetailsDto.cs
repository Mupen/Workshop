using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record UpdateProductDetailsDto(
    [property: Required]
    [property: MaxLength(64)]
    string Sku,
    [property: Required]
    [property: MaxLength(200)]
    string Name,
    [property: MaxLength(1000)]
    string Description);
