using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class UpdateProductDetails
{
    private readonly IProductRepository _productRepository;

    public UpdateProductDetails(IProductRepository productRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Updates product text details after checking that the product exists and the SKU is not used by another product.
    /// </summary>
    public async Task<Result> ExecuteAsync(UpdateProductDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure(
                new Error("Product.NotFound", "Product was not found."));
        }

        var existing = await _productRepository.GetBySkuAsync(request.Sku, cancellationToken);
        if (existing is not null && existing.Id != product.Id)
        {
            return Result.Failure(
                new Error("Product.DuplicateSku", $"Product SKU '{request.Sku}' already exists."));
        }

        var result = product.UpdateDetails(request.Sku, request.Name, request.Description);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
