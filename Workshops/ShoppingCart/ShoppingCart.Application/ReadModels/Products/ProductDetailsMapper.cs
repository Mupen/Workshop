using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.ReadModels.Products;

internal static class ProductDetailsMapper
{
    private const string MissingBrandName = "(missing brand)";
    private const string MissingCategoryName = "(missing category)";

    /// <summary>
    /// Builds a ProductDetails read model from a product and its optional brand/category.
    /// </summary>
    public static ProductDetails ToDetails(
        Product product,
        ProductBrand? brand,
        ProductCategory? category)
    {
        return new ProductDetails(
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            product.BrandId,
            brand?.Name ?? MissingBrandName,
            product.CategoryId,
            category?.Name ?? MissingCategoryName,
            product.UnitPrice,
            product.TrackInventory,
            product.StockQuantity,
            product.Status,
            product.IsAvailable);
    }
}
