using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Queries.ProductBrands;

public sealed class GetProductBrandById
{
    private readonly IProductBrandRepository _brandRepository;

    public GetProductBrandById(IProductBrandRepository brandRepository)
    {
        _brandRepository = brandRepository
            ?? throw new ArgumentNullException(nameof(brandRepository));
    }

    /// <summary>
    /// Gets one product brand by id, or returns null if it does not exist.
    /// </summary>
    public Task<ProductBrand?> ExecuteAsync(Guid brandId, CancellationToken cancellationToken = default)
        => _brandRepository.GetByIdAsync(brandId, cancellationToken);
}
