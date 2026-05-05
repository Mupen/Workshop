using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Requests.ProductBrands;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Application.UseCases.ProductBrands;

public sealed class RenameProductBrand
{
    private readonly IProductBrandRepository _brandRepository;

    public RenameProductBrand(IProductBrandRepository brandRepository)
    {
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
    }

    /// <summary>
    /// Renames a product brand after checking that the brand exists and the new name is not taken.
    /// </summary>
    public async Task<Result> ExecuteAsync(RenameProductBrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
        {
            return Result.Failure(
                new Error("ProductBrand.NotFound", "Product brand was not found."));
        }

        var existing = await _brandRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null && existing.Id != brand.Id)
        {
            return Result.Failure(
                new Error("ProductBrand.DuplicateName", $"Product brand '{request.Name}' already exists."));
        }

        var result = brand.Rename(request.Name);
        if (result.IsFailure)
            return result;

        await _brandRepository.UpdateAsync(brand, cancellationToken);
        return Result.Success();
    }
}
