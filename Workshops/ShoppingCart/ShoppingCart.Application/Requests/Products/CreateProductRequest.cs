using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Application.Requests.Products;

public sealed record CreateProductRequest(
    string Sku,
    string Name,
    string Description,
    Guid BrandId,
    Guid CategoryId,
    decimal UnitPrice,
    bool TrackInventory,
    int? StockQuantity,
    ProductStatus Status = ProductStatus.Active);
