using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ProductCategories;
using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.UseCases.ProductCategories;

public sealed class CreateProductCategory
{
    private readonly IProductCategoryRepository _categoryRepository;

    public CreateProductCategory(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Creates a product category if the name is free and the optional parent category exists.
    /// </summary>
    public async Task<Result<ProductCategory>> ExecuteAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null)
        {
            return Result<ProductCategory>.Failure(
                new Error("ProductCategory.DuplicateName", $"Product category '{request.Name}' already exists."));
        }

        if (request.ParentCategoryId is not null)
        {
            var parent = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value, cancellationToken);
            if (parent is null)
            {
                return Result<ProductCategory>.Failure(
                    new Error("ProductCategory.ParentNotFound", "Parent product category was not found."));
            }
        }

        var result = ProductCategory.Create(request.Name, request.ParentCategoryId);
        if (result.IsFailure)
            return result;

        await _categoryRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
