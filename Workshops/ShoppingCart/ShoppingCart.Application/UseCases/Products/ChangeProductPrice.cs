using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class ChangeProductPrice
{
    private readonly IProductRepository _productRepository;

    public ChangeProductPrice(IProductRepository productRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <summary>
    /// Changes a product's price after loading the product and checking the price rule.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeProductPriceRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure(new Error("Product.NotFound", "Product was not found."));

        var result = product.ChangePrice(request.UnitPrice);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
