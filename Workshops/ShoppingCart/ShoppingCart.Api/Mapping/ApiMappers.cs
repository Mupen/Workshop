using ShoppingCart.Api.Contracts.ProductBrands;
using ShoppingCart.Api.Contracts.ProductCategories;
using ShoppingCart.Api.Contracts.Products;
using ShoppingCart.Api.Contracts.ShoppingCarts;
using ShoppingCart.Application.ReadModels.Products;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Api.Mapping;

internal static class ApiMappers
{
    /// <summary>
    /// Converts a ProductBrand domain object into the response shape returned by the API.
    /// </summary>
    public static ProductBrandResponse ToResponse(this ProductBrand brand)
        => new(brand.Id, brand.Name);

    /// <summary>
    /// Converts a ProductCategory domain object into the response shape returned by the API.
    /// </summary>
    public static ProductCategoryResponse ToResponse(this ProductCategory category)
        => new(category.Id, category.Name, category.ParentCategoryId, category.IsRootCategory);

    /// <summary>
    /// Converts a product read model into the response shape returned by the API.
    /// </summary>
    public static ProductResponse ToResponse(this ProductDetails product)
    {
        return new ProductResponse(
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            product.BrandId,
            product.BrandName,
            product.CategoryId,
            product.CategoryName,
            product.UnitPrice,
            product.TrackInventory,
            product.StockQuantity,
            product.Status,
            product.IsAvailable);
    }

    /// <summary>
    /// Converts a ShoppingCart domain object and its items into the response shape returned by the API.
    /// </summary>
    public static ShoppingCartResponse ToResponse(this ShoppingCart.Domain.Entities.ShoppingCart cart)
    {
        var items = cart.Items
            .Select(item => new ShoppingCartItemResponse(
                item.Id,
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                item.LineTotal))
            .ToList();

        return new ShoppingCartResponse(
            cart.Id,
            cart.UserId,
            cart.TotalQuantity,
            cart.TotalPrice,
            items);
    }
}
