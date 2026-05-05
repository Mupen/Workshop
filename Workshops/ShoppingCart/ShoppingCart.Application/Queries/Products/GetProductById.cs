using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ReadModels.Products;

namespace ShoppingCart.Application.Queries.Products;

public sealed class GetProductById
{
    private readonly IProductRepository _productRepository;
    private readonly IProductBrandRepository _brandRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public GetProductById(
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
    /// Gets one product by id and adds brand/category names for easy display.
    /// </summary>
    public async Task<ProductDetails?> ExecuteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return null;

        var brand = await _brandRepository.GetByIdAsync(product.BrandId, cancellationToken);
        var category = await _categoryRepository.GetByIdAsync(product.CategoryId, cancellationToken);

        return ProductDetailsMapper.ToDetails(product, brand, category);
    }
}
