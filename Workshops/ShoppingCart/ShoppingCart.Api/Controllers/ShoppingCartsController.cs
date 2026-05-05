using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Contracts.ShoppingCarts;
using ShoppingCart.Api.Mapping;
using ShoppingCart.Application.Queries.ShoppingCarts;
using ShoppingCart.Application.Requests.ShoppingCarts;
using ShoppingCart.Application.UseCases.ShoppingCarts;

namespace ShoppingCart.Api.Controllers;

[ApiController]
[Route("api/shopping-carts")]
public sealed class ShoppingCartsController : ControllerBase
{
    /// <summary>
    /// Gets every shopping cart from the database and returns them as JSON.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ShoppingCartResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllShoppingCarts query, CancellationToken cancellationToken)
    {
        var carts = await query.ExecuteAsync(cancellationToken);
        return Ok(carts.Select(cart => cart.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets one shopping cart by id, or returns 404 if it does not exist.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetShoppingCartById query, CancellationToken cancellationToken)
    {
        var cart = await query.ExecuteAsync(id, cancellationToken);
        return cart is null ? NotFound() : Ok(cart.ToResponse());
    }

    /// <summary>
    /// Gets all shopping carts that belong to one user.
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ShoppingCartResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId, [FromServices] GetShoppingCartsByUserId query, CancellationToken cancellationToken)
    {
        var carts = await query.ExecuteAsync(userId, cancellationToken);
        return Ok(carts.Select(cart => cart.ToResponse()).ToList());
    }

    /// <summary>
    /// Creates a new empty shopping cart for a user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateShoppingCartDto dto,
        [FromServices] CreateShoppingCart useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new CreateShoppingCartRequest(dto.UserId), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value.ToResponse());
    }

    /// <summary>
    /// Adds, updates, or removes a product item in a shopping cart.
    /// </summary>
    [HttpPut("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetItem(
        Guid id,
        SetShoppingCartItemDto dto,
        [FromServices] SetShoppingCartItem useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(new SetShoppingCartItemRequest(id, dto.ProductId, dto.Quantity), cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }

    /// <summary>
    /// Deletes an existing shopping cart.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteShoppingCart useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : this.ToProblem(result);
    }
}
