using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record AdjustProductStockDto(
    [property: Range(1, int.MaxValue)]
    int Quantity);
