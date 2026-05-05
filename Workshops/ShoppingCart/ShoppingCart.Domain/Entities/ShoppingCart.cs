using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Domain.Entities;

public sealed class ShoppingCart
{
    private readonly List<ShoppingCartItem> _items = [];

    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private ShoppingCart()
    {
    }

    /// <summary>
    /// Creates a shopping cart object after the public factory method has checked the rules.
    /// </summary>
    private ShoppingCart(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    public int TotalQuantity => _items.Sum(item => item.Quantity);
    public decimal TotalPrice => decimal.Round(_items.Sum(item => item.LineTotal), 2, MidpointRounding.AwayFromZero);
    public bool IsEmpty => _items.Count == 0;

    /// <summary>
    /// Creates a new empty shopping cart for a user.
    /// </summary>
    public static Result<ShoppingCart> Create(Guid userId)
    {
        var result = ValidateUserId(userId);
        if (result.IsFailure)
            return result.ToFailure<ShoppingCart>();

        var cart = new ShoppingCart(Guid.NewGuid(), userId);
        return Result<ShoppingCart>.Success(cart);
    }

    /// <summary>
    /// Adds a product to the cart, changes its quantity, or removes it when quantity is zero.
    /// </summary>
    public Result SetItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity == 0)
            return RemoveItem(product.Id);

        var existingItem = _items.SingleOrDefault(item => item.ProductId == product.Id);
        if (existingItem is null)
        {
            var createResult = ShoppingCartItem.Create(product, quantity);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            _items.Add(createResult.Value);
            return Result.Success();
        }

        return existingItem.UpdateFromProduct(product, quantity);
    }

    /// <summary>
    /// Removes one product from the cart by product id.
    /// </summary>
    public Result RemoveItem(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            return Result.Failure(
                new Error("ShoppingCart.InvalidProductId", "Product id is required."));
        }

        var existingItem = _items.SingleOrDefault(item => item.ProductId == productId);
        if (existingItem is null)
        {
            return Result.Failure(
                new Error("ShoppingCart.ItemNotFound", "Shopping cart item was not found."));
        }

        _items.Remove(existingItem);
        return Result.Success();
    }

    /// <summary>
    /// Removes all items from the cart.
    /// </summary>
    public Result Clear()
    {
        if (_items.Count == 0)
        {
            return Result.Failure(
                new Error("ShoppingCart.AlreadyEmpty", "Shopping cart is already empty."));
        }

        _items.Clear();
        return Result.Success();
    }

    /// <summary>
    /// Checks that the cart belongs to a real user id.
    /// </summary>
    private static Result ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure(
                new Error("ShoppingCart.InvalidUserId", "Shopping cart user id is required."));
        }

        return Result.Success();
    }
}
