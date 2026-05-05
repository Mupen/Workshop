using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record ChangeProductStatusDto(ProductStatus Status);
