using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts.ProductCategories;
using ShoppingCart.Api.Mapping;
using ShoppingCart.Application.Queries.ProductCategories;
using ShoppingCart.Application.Requests.ProductCategories;
using ShoppingCart.Application.UseCases.ProductCategories;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("api/product-categories")]
public sealed class ProductCategoriesController : ControllerBase
{
    /// <summary>
    /// Gets every product category from the database and returns them as JSON.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllProductCategories query, CancellationToken cancellationToken)
    {
        var categories = await query.ExecuteAsync(cancellationToken);
        return Ok(categories.Select(category => category.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets one product category by id, or returns 404 if it does not exist.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetProductCategoryById query, CancellationToken cancellationToken)
    {
        var category = await query.ExecuteAsync(id, cancellationToken);
        return category is null ? NotFound() : Ok(category.ToResponse());
    }

    /// <summary>
    /// Creates a new product category, optionally under a parent category.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductCategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        CreateProductCategoryDto dto,
        [FromServices] CreateProductCategory useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new CreateProductCategoryRequest(dto.Name, dto.ParentCategoryId), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value.ToResponse());
    }

    /// <summary>
    /// Changes the name of an existing product category.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Rename(
        Guid id,
        RenameProductCategoryDto dto,
        [FromServices] RenameProductCategory useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new RenameProductCategoryRequest(id, dto.Name), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Moves a category under a different parent, or makes it a root category.
    /// </summary>
    [HttpPatch("{id:guid}/parent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Move(
        Guid id,
        MoveProductCategoryDto dto,
        [FromServices] MoveProductCategory useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new MoveProductCategoryRequest(id, dto.ParentCategoryId), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }
}
