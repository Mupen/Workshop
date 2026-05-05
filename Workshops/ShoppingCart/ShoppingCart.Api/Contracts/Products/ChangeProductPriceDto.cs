using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record ChangeProductPriceDto(
    [property: Range(typeof(decimal), "0", "79228162514264337593543950335")]
    decimal UnitPrice);
