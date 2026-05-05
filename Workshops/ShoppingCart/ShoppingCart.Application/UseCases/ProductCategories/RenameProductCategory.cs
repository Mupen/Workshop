using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ProductCategories;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ProductCategories;

public sealed class RenameProductCategory
{
    private readonly IProductCategoryRepository _categoryRepository;

    public RenameProductCategory(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Renames a product category after checking that the category exists and the new name is not taken.
    /// </summary>
    public async Task<Result> ExecuteAsync(RenameProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result.Failure(
                new Error("ProductCategory.NotFound", "Product category was not found."));
        }

        var existing = await _categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null && existing.Id != category.Id)
        {
            return Result.Failure(
                new Error("ProductCategory.DuplicateName", $"Product category '{request.Name}' already exists."));
        }

        var result = category.Rename(request.Name);
        if (result.IsFailure)
            return result;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
        return Result.Success();
    }
}
