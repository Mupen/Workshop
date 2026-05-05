using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class CreateProduct
{
    private readonly IProductRepository _productRepository;
    private readonly IProductBrandRepository _brandRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public CreateProduct(
        IProductRepository productRepository,
        IProductBrandRepository brandRepository,
        IProductCategoryRepository categoryRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Creates a product after checking duplicate SKU, brand, category, and product rules.
    /// </summary>
    public async Task<Result<Product>> ExecuteAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _productRepository.GetBySkuAsync(request.Sku, cancellationToken);
        if (existing is not null)
        {
            return Result<Product>.Failure(
                new Error("Product.DuplicateSku", $"Product SKU '{request.Sku}' already exists."));
        }

        var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
        {
            return Result<Product>.Failure(
                new Error("ProductBrand.NotFound", "Product brand was not found."));
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result<Product>.Failure(
                new Error("ProductCategory.NotFound", "Product category was not found."));
        }

        var result = Product.Create(
            request.Sku,
            request.Name,
            request.Description,
            request.BrandId,
            request.CategoryId,
            request.UnitPrice,
            request.TrackInventory,
            request.StockQuantity,
            request.Status);

        if (result.IsFailure)
            return result;

        await _productRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
