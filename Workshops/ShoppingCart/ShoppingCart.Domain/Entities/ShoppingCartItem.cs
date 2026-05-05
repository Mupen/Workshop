using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Domain.Entities;

public sealed class ShoppingCartItem
{
    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private ShoppingCartItem()
    {
        ProductName = string.Empty;
    }

    /// <summary>
    /// Creates a cart item object after the factory method has checked the rules.
    /// </summary>
    private ShoppingCartItem(Guid id, Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = RoundMoney(unitPrice);
        Quantity = quantity;
    }

    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal LineTotal => RoundMoney(UnitPrice * Quantity);

    /// <summary>
    /// Creates a cart item from a product and quantity.
    /// </summary>
    internal static Result<ShoppingCartItem> Create(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        var result = product.CanSupply(quantity);
        if (result.IsFailure)
            return result.ToFailure<ShoppingCartItem>();

        var item = new ShoppingCartItem(
            Guid.NewGuid(),
            product.Id,
            product.Name,
            product.UnitPrice,
            quantity);

        return Result<ShoppingCartItem>.Success(item);
    }

    /// <summary>
    /// Updates this cart item with the latest product name, price, and requested quantity.
    /// </summary>
    internal Result UpdateFromProduct(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Id != ProductId)
        {
            return Result.Failure(
                new Error("ShoppingCartItem.ProductMismatch", "Product does not match this shopping cart item."));
        }

        var result = product.CanSupply(quantity);
        if (result.IsFailure)
            return result;

        ProductName = product.Name;
        UnitPrice = RoundMoney(product.UnitPrice);
        Quantity = quantity;

        return Result.Success();
    }

    /// <summary>
    /// Rounds money to two decimals in a predictable way.
    /// </summary>
    private static decimal RoundMoney(decimal value)
        => decimal.Round(value, 2, MidpointRounding.AwayFromZero);
}
