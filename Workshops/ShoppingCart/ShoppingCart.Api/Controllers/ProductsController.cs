using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts.Products;
using ShoppingCart.Api.Mapping;
using ShoppingCart.Application.Queries.Products;
using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Application.UseCases.Products;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    /// <summary>
    /// Gets every product from the database and returns them as JSON.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllProducts query, CancellationToken cancellationToken)
    {
        var products = await query.ExecuteAsync(cancellationToken);
        return Ok(products.Select(product => product.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets only products that can currently be bought.
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable([FromServices] GetAvailableProducts query, CancellationToken cancellationToken)
    {
        var products = await query.ExecuteAsync(cancellationToken);
        return Ok(products.Select(product => product.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets one product by id, or returns 404 if it does not exist.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetProductById query, CancellationToken cancellationToken)
    {
        var product = await query.ExecuteAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product.ToResponse());
    }

    /// <summary>
    /// Creates a new product after checking brand, category, and product rules.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        CreateProductDto dto,
        [FromServices] CreateProduct useCase,
        [FromServices] GetProductById query,
        CancellationToken cancellationToken)
    {
        var request = new CreateProductRequest(
            dto.Sku,
            dto.Name,
            dto.Description,
            dto.BrandId,
            dto.CategoryId,
            dto.UnitPrice,
            dto.TrackInventory,
            dto.StockQuantity,
            dto.Status);

        var result = await useCase.ExecuteAsync(request, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result);

        var product = await query.ExecuteAsync(result.Value.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, product?.ToResponse());
    }

    /// <summary>
    /// Updates the basic text details of an existing product.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateDetails(
        Guid id,
        UpdateProductDetailsDto dto,
        [FromServices] UpdateProductDetails useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new UpdateProductDetailsRequest(id, dto.Sku, dto.Name, dto.Description), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Changes which brand an existing product belongs to.
    /// </summary>
    [HttpPatch("{id:guid}/brand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeBrand(
        Guid id,
        ChangeProductBrandDto dto,
        [FromServices] ChangeProductBrand useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new ChangeProductBrandRequest(id, dto.BrandId), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Changes which category an existing product belongs to.
    /// </summary>
    [HttpPatch("{id:guid}/category")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeCategory(
        Guid id,
        ChangeProductCategoryDto dto,
        [FromServices] ChangeProductCategory useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new ChangeProductCategoryRequest(id, dto.CategoryId), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Changes the price of an existing product.
    /// </summary>
    [HttpPatch("{id:guid}/price")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePrice(
        Guid id,
        ChangeProductPriceDto dto,
        [FromServices] ChangeProductPrice useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new ChangeProductPriceRequest(id, dto.UnitPrice), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Changes whether a product is active, inactive, or discontinued.
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        ChangeProductStatusDto dto,
        [FromServices] ChangeProductStatus useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new ChangeProductStatusRequest(id, dto.Status), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Adds more stock to a product that tracks inventory.
    /// </summary>
    [HttpPatch("{id:guid}/stock/increase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IncreaseStock(
        Guid id,
        AdjustProductStockDto dto,
        [FromServices] IncreaseProductStock useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new AdjustProductStockRequest(id, dto.Quantity), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Removes stock from a product that tracks inventory.
    /// </summary>
    [HttpPatch("{id:guid}/stock/decrease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DecreaseStock(
        Guid id,
        AdjustProductStockDto dto,
        [FromServices] DecreaseProductStock useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new AdjustProductStockRequest(id, dto.Quantity), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }
}
