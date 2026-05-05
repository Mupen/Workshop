using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Domain.Entities;

public sealed class ProductBrand
{
    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private ProductBrand()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Creates a brand object after the public factory method has checked the rules.
    /// </summary>
    private ProductBrand(Guid id, string name)
    {
        Id = id;
        Name = name.Trim();
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    /// <summary>
    /// Creates a new brand if the name is valid.
    /// </summary>
    public static Result<ProductBrand> Create(string name)
    {
        var result = ValidateName(name);
        if (result.IsFailure)
            return result.ToFailure<ProductBrand>();

        var brand = new ProductBrand(Guid.NewGuid(), name);
        return Result<ProductBrand>.Success(brand);
    }

    /// <summary>
    /// Changes the brand name if the new name is valid.
    /// </summary>
    public Result Rename(string name)
    {
        var result = ValidateName(name);
        if (result.IsFailure)
            return result;

        Name = name.Trim();
        return Result.Success();
    }

    /// <summary>
    /// Checks that a brand name is not empty and is not too long.
    /// </summary>
    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(
                new Error("ProductBrand.InvalidName", "Product brand name is required."));
        }

        if (name.Trim().Length > 100)
        {
            return Result.Failure(
                new Error("ProductBrand.NameTooLong", "Product brand name cannot be longer than 100 characters."));
        }

        return Result.Success();
    }
}
