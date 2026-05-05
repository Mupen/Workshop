using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.ProductBrands;

public sealed class GetAllProductBrands
{
    private readonly IProductBrandRepository _brandRepository;

    public GetAllProductBrands(IProductBrandRepository brandRepository)
    {
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
    }

    /// <summary>
    /// Gets all product brands from the repository.
    /// </summary>
    public Task<IReadOnlyList<ProductBrand>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _brandRepository.GetAllAsync(cancellationToken);
}
