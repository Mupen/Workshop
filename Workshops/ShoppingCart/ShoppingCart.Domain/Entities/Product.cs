using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Domain.Entities;

public sealed class Product
{
    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private Product()
    {
        Sku = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
    }

    /// <summary>
    /// Creates a product object after the public factory method has checked the rules.
    /// </summary>
    private Product(
        Guid id,
        string sku,
        string name,
        string description,
        Guid brandId,
        Guid categoryId,
        decimal unitPrice,
        bool trackInventory,
        int? stockQuantity,
        ProductStatus status)
    {
        Id = id;
        Sku = sku.Trim();
        Name = name.Trim();
        Description = NormalizeDescription(description);
        BrandId = brandId;
        CategoryId = categoryId;
        UnitPrice = RoundMoney(unitPrice);
        TrackInventory = trackInventory;
        StockQuantity = stockQuantity;
        Status = status;
    }

    public Guid Id { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public bool TrackInventory { get; private set; }
    public int? StockQuantity { get; private set; }
    public ProductStatus Status { get; private set; }
    public bool IsActive => Status == ProductStatus.Active;
    public bool IsAvailable => IsActive && (!TrackInventory || StockQuantity > 0);

    /// <summary>
    /// Creates a new product if all product rules are valid.
    /// </summary>
    public static Result<Product> Create(
        string sku,
        string name,
        string description,
        Guid brandId,
        Guid categoryId,
        decimal unitPrice,
        bool trackInventory,
        int? stockQuantity,
        ProductStatus status = ProductStatus.Active)
    {
        var result = ValidateSku(sku);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateName(name);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateDescription(description);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateBrandId(brandId);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateCategoryId(categoryId);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateUnitPrice(unitPrice);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateInventory(trackInventory, stockQuantity);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        result = ValidateStatus(status);
        if (result.IsFailure)
            return result.ToFailure<Product>();

        var product = new Product(
            Guid.NewGuid(),
            sku,
            name,
            description,
            brandId,
            categoryId,
            unitPrice,
            trackInventory,
            stockQuantity,
            status);

        return Result<Product>.Success(product);
    }

    /// <summary>
    /// Changes the SKU, name, and description if all new values are valid.
    /// </summary>
    public Result UpdateDetails(string sku, string name, string description)
    {
        var result = ValidateSku(sku);
        if (result.IsFailure)
            return result;

        result = ValidateName(name);
        if (result.IsFailure)
            return result;

        result = ValidateDescription(description);
        if (result.IsFailure)
            return result;

        Sku = sku.Trim();
        Name = name.Trim();
        Description = NormalizeDescription(description);

        return Result.Success();
    }

    /// <summary>
    /// Changes the product brand if the brand id is valid.
    /// </summary>
    public Result ChangeBrand(Guid brandId)
    {
        var result = ValidateBrandId(brandId);
        if (result.IsFailure)
            return result;

        BrandId = brandId;
        return Result.Success();
    }

    /// <summary>
    /// Changes the product category if the category id is valid.
    /// </summary>
    public Result ChangeCategory(Guid categoryId)
    {
        var result = ValidateCategoryId(categoryId);
        if (result.IsFailure)
            return result;

        CategoryId = categoryId;
        return Result.Success();
    }

    /// <summary>
    /// Changes the product price if the price is not negative.
    /// </summary>
    public Result ChangePrice(decimal unitPrice)
    {
        var result = ValidateUnitPrice(unitPrice);
        if (result.IsFailure)
            return result;

        UnitPrice = RoundMoney(unitPrice);
        return Result.Success();
    }

    /// <summary>
    /// Adds stock to the product if this product tracks inventory.
    /// </summary>
    public Result IncreaseStock(int quantity)
    {
        if (!TrackInventory)
        {
            return Result.Failure(
                new Error("Product.InventoryNotTracked", "Stock cannot be changed for products that do not track inventory."));
        }

        if (quantity <= 0)
        {
            return Result.Failure(
                new Error("Product.InvalidStockIncreaseQuantity", "Stock increase quantity must be greater than zero."));
        }

        StockQuantity += quantity;
        return Result.Success();
    }

    /// <summary>
    /// Removes stock from the product if enough stock is available.
    /// </summary>
    public Result DecreaseStock(int quantity)
    {
        if (!TrackInventory)
        {
            return Result.Failure(
                new Error("Product.InventoryNotTracked", "Stock cannot be changed for products that do not track inventory."));
        }

        if (quantity <= 0)
        {
            return Result.Failure(
                new Error("Product.InvalidStockDecreaseQuantity", "Stock decrease quantity must be greater than zero."));
        }

        if (quantity > StockQuantity.GetValueOrDefault())
        {
            return Result.Failure(
                new Error("Product.InsufficientStock", "Not enough stock is available."));
        }

        StockQuantity -= quantity;
        return Result.Success();
    }

    /// <summary>
    /// Changes the product status if the status value exists in the enum.
    /// </summary>
    public Result ChangeStatus(ProductStatus status)
    {
        var result = ValidateStatus(status);
        if (result.IsFailure)
            return result;

        Status = status;
        return Result.Success();
    }

    /// <summary>
    /// Checks whether this product can supply the requested quantity for a shopping cart.
    /// </summary>
    public Result CanSupply(int quantity)
    {
        if (!IsActive)
        {
            return Result.Failure(
                new Error("Product.NotActive", "Only active products can be added to a shopping cart."));
        }

        if (quantity <= 0)
        {
            return Result.Failure(
                new Error("Product.InvalidRequestedQuantity", "Requested quantity must be greater than zero."));
        }

        if (TrackInventory && quantity > StockQuantity.GetValueOrDefault())
        {
            return Result.Failure(
                new Error("Product.InsufficientStock", "Requested quantity exceeds available stock."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the SKU is not empty and is not too long.
    /// </summary>
    private static Result ValidateSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return Result.Failure(
                new Error("Product.InvalidSku", "Product SKU is required."));
        }

        if (sku.Trim().Length > 64)
        {
            return Result.Failure(
                new Error("Product.SkuTooLong", "Product SKU cannot be longer than 64 characters."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the product name is not empty and is not too long.
    /// </summary>
    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(
                new Error("Product.InvalidName", "Product name is required."));
        }

        if (name.Trim().Length > 200)
        {
            return Result.Failure(
                new Error("Product.NameTooLong", "Product name cannot be longer than 200 characters."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the description is not too long.
    /// </summary>
    private static Result ValidateDescription(string description)
    {
        if (NormalizeDescription(description).Length > 1000)
        {
            return Result.Failure(
                new Error("Product.DescriptionTooLong", "Product description cannot be longer than 1000 characters."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the product has a real brand id.
    /// </summary>
    private static Result ValidateBrandId(Guid brandId)
    {
        if (brandId == Guid.Empty)
        {
            return Result.Failure(
                new Error("Product.InvalidBrandId", "Product brand id is required."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the product has a real category id.
    /// </summary>
    private static Result ValidateCategoryId(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
        {
            return Result.Failure(
                new Error("Product.InvalidCategoryId", "Product category id is required."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the product price is not negative.
    /// </summary>
    private static Result ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
        {
            return Result.Failure(
                new Error("Product.InvalidUnitPrice", "Product unit price cannot be negative."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that stock quantity matches whether inventory is tracked.
    /// </summary>
    private static Result ValidateInventory(bool trackInventory, int? stockQuantity)
    {
        if (trackInventory && stockQuantity is null)
        {
            return Result.Failure(
                new Error("Product.StockQuantityRequired", "Stock quantity is required when inventory is tracked."));
        }

        if (!trackInventory && stockQuantity is not null)
        {
            return Result.Failure(
                new Error("Product.StockQuantityNotAllowed", "Stock quantity must be empty when inventory is not tracked."));
        }

        if (stockQuantity < 0)
        {
            return Result.Failure(
                new Error("Product.InvalidStockQuantity", "Product stock quantity cannot be negative."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the product status is a valid enum value.
    /// </summary>
    private static Result ValidateStatus(ProductStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            return Result.Failure(
                new Error("Product.InvalidStatus", "Product status is not valid."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Rounds money to two decimals in a predictable way.
    /// </summary>
    private static decimal RoundMoney(decimal value)
        => decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    /// <summary>
    /// Trims the description and turns null into an empty string.
    /// </summary>
    private static string NormalizeDescription(string description)
        => description?.Trim() ?? string.Empty;
}
