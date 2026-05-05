using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ReadModels.Products;

namespace ShoppingCart.Application.Queries.Products;

public sealed class GetAllProducts
{
    private readonly IProductRepository _productRepository;
    private readonly IProductBrandRepository _brandRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public GetAllProducts(
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
    /// Gets all products and adds brand/category names for easy display.
    /// </summary>
    public async Task<IReadOnlyList<ProductDetails>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var brands = await _brandRepository.GetAllAsync(cancellationToken);
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        var brandsById = brands.ToDictionary(brand => brand.Id);
        var categoriesById = categories.ToDictionary(category => category.Id);

        return products
            .Select(product =>
            {
                brandsById.TryGetValue(product.BrandId, out var brand);
                categoriesById.TryGetValue(product.CategoryId, out var category);

                return ProductDetailsMapper.ToDetails(product, brand, category);
            })
            .ToList();
    }
}
