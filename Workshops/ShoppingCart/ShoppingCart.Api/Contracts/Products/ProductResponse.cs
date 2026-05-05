using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Api.Contracts.Products;

public sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string Description,
    Guid BrandId,
    string BrandName,
    Guid CategoryId,
    string CategoryName,
    decimal UnitPrice,
    bool TrackInventory,
    int? StockQuantity,
    ProductStatus Status,
    bool IsAvailable);
