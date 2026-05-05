using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ProductBrands;
using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.UseCases.ProductBrands;

public sealed class CreateProductBrand
{
    private readonly IProductBrandRepository _brandRepository;

    public CreateProductBrand(IProductBrandRepository brandRepository)
    {
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
    }

    /// <summary>
    /// Creates a product brand if another brand with the same name does not already exist.
    /// </summary>
    public async Task<Result<ProductBrand>> ExecuteAsync(CreateProductBrandRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _brandRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null)
        {
            return Result<ProductBrand>.Failure(
                new Error("ProductBrand.DuplicateName", $"Product brand '{request.Name}' already exists."));
        }

        var result = ProductBrand.Create(request.Name);
        if (result.IsFailure)
            return result;

        await _brandRepository.AddAsync(result.Value, cancellationToken);
        return result;
    }
}
