using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class ChangeProductStatus
{
    private readonly IProductRepository _productRepository;

    public ChangeProductStatus(IProductRepository productRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Changes a product's status, such as active, inactive, or discontinued.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeProductStatusRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure(new Error("Product.NotFound", "Product was not found."));

        var result = product.ChangeStatus(request.Status);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
