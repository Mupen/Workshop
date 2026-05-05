using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.ProductCategories;

public sealed class GetAllProductCategories
{
    private readonly IProductCategoryRepository _categoryRepository;

    public GetAllProductCategories(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Gets all product categories from the repository.
    /// </summary>
    public Task<IReadOnlyList<ProductCategory>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _categoryRepository.GetAllAsync(cancellationToken);
}
