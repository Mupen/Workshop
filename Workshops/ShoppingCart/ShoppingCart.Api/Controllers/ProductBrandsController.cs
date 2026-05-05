using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts.ProductBrands;
using ShoppingCart.Api.Mapping;
using ShoppingCart.Application.Queries.ProductBrands;
using ShoppingCart.Application.Requests.ProductBrands;
using ShoppingCart.Application.UseCases.ProductBrands;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("api/product-brands")]
public sealed class ProductBrandsController : ControllerBase
{
    /// <summary>
    /// Gets every product brand from the database and returns them as JSON.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductBrandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllProductBrands query, CancellationToken cancellationToken)
    {
        var brands = await query.ExecuteAsync(cancellationToken);
        return Ok(brands.Select(brand => brand.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets one product brand by id, or returns 404 if it does not exist.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductBrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetProductBrandById query, CancellationToken cancellationToken)
    {
        var brand = await query.ExecuteAsync(id, cancellationToken);
        return brand is null ? NotFound() : Ok(brand.ToResponse());
    }

    /// <summary>
    /// Creates a new product brand from the request body.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductBrandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateProductBrandDto dto, [FromServices] CreateProductBrand useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new CreateProductBrandRequest(dto.Name), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value.ToResponse());
    }

    /// <summary>
    /// Changes the name of an existing product brand.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Rename(
        Guid id,
        RenameProductBrandDto dto,
        [FromServices] RenameProductBrand useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new RenameProductBrandRequest(id, dto.Name), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }
}
