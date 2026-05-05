using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.ProductCategories;

public sealed class GetProductCategoryById
{
    private readonly IProductCategoryRepository _categoryRepository;

    public GetProductCategoryById(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Gets one product category by id, or returns null if it does not exist.
    /// </summary>
    public Task<ProductCategory?> ExecuteAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
}
