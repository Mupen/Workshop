using System.ComponentModel.DataAnnotations;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record CreateProductDto(
    [property: Required]
    [property: MaxLength(64)]
    string Sku,
    [property: Required]
    [property: MaxLength(200)]
    string Name,
    [property: MaxLength(1000)]
    string Description,
    Guid BrandId,
    Guid CategoryId,
    [property: Range(typeof(decimal), "0", "79228162514264337593543950335")]
    decimal UnitPrice,
    bool TrackInventory,
    [property: Range(0, int.MaxValue)]
    int? StockQuantity,
    ProductStatus Status = ProductStatus.Active);
