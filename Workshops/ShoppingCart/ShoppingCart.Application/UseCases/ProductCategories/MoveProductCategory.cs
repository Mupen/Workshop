using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ProductCategories;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ProductCategories;

public sealed class MoveProductCategory
{
    private readonly IProductCategoryRepository _categoryRepository;

    public MoveProductCategory(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Moves a category to another parent category, or removes the parent to make it a root category.
    /// </summary>
    public async Task<Result> ExecuteAsync(MoveProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result.Failure(
                new Error("ProductCategory.NotFound", "Product category was not found."));
        }

        if (request.ParentCategoryId is not null)
        {
            var parent = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value, cancellationToken);
            if (parent is null)
            {
                return Result.Failure(
                    new Error("ProductCategory.ParentNotFound", "Parent product category was not found."));
            }
        }

        var result = category.MoveTo(request.ParentCategoryId);
        if (result.IsFailure)
            return result;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
        return Result.Success();
    }
}
