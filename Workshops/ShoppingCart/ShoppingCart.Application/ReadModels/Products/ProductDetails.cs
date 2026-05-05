using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.ReadModels.Products;

/// <summary>
/// A read model used when the API needs product data together with display names for brand and category.
/// </summary>
public sealed record ProductDetails(
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
