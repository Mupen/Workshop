using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.Products;

public sealed class ChangeProductCategory
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public ChangeProductCategory(IProductRepository productRepository, IProductCategoryRepository categoryRepository)
    {
        _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Changes a product's category after checking that both the product and new category exist.
    /// </summary>
    public async Task<Result> ExecuteAsync(ChangeProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result.Failure(new Error("Product.NotFound", "Product was not found."));

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Result.Failure(new Error("ProductCategory.NotFound", "Product category was not found."));

        var result = product.ChangeCategory(request.CategoryId);
        if (result.IsFailure)
            return result;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
