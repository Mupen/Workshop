using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class ChangeProductBrand
{
    private readonly IProductRepository _productRepository;
    private readonly IProductBrandRepository _brandRepository;

    public ChangeProductBrand(IProductRepository productRepository, IProductBrandRepository brandRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
    }

    /// <summary>
    /// Changes a product's brand after checking that both the product and new brand exist.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeProductBrandRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure(new Error("Product.NotFound", "Product was not found."));

        var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
            return Result.Failure(new Error("ProductBrand.NotFound", "Product brand was not found."));

        var result = product.ChangeBrand(request.BrandId);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
