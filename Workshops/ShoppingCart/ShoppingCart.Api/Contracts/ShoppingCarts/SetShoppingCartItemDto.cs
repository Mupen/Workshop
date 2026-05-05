using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.ShoppingCarts;

public sealed record SetShoppingCartItemDto(
    Guid ProductId,
    [property: Range(0, int.MaxValue)]
    int Quantity);
