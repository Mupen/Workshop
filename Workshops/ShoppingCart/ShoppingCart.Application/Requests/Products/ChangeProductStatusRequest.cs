using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.Requests.Products;

public sealed record ChangeProductStatusRequest(Guid ProductId, ProductStatus Status);
