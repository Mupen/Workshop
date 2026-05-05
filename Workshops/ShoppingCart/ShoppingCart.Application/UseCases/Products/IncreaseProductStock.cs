using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class IncreaseProductStock
{
    private readonly IProductRepository _productRepository;

    public IncreaseProductStock(IProductRepository productRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Increases product stock if the product exists and tracks inventory.
    /// </summary>
    public async Task<Result> ExecuteAsync(AdjustProductStockRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure(new Error("Product.NotFound", "Product was not found."));

        var result = product.IncreaseStock(request.Quantity);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
