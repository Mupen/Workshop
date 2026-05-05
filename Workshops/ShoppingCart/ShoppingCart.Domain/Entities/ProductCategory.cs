using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Domain.Entities;

public sealed class ProductCategory
{
    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private ProductCategory()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Creates a category object after the public factory method has checked the rules.
    /// </summary>
    private ProductCategory(Guid id, string name, Guid? parentCategoryId)
    {
        Id = id;
        Name = name.Trim();
        ParentCategoryId = parentCategoryId;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public bool IsRootCategory => ParentCategoryId is null;

    /// <summary>
    /// Creates a new category if the name and optional parent id are valid.
    /// </summary>
    public static Result<ProductCategory> Create(string name, Guid? parentCategoryId = null)
    {
        var result = ValidateName(name);
        if (result.IsFailure)
            return result.ToFailure<ProductCategory>();

        result = ValidateParentCategoryId(parentCategoryId);
        if (result.IsFailure)
            return result.ToFailure<ProductCategory>();

        var category = new ProductCategory(Guid.NewGuid(), name, parentCategoryId);
        return Result<ProductCategory>.Success(category);
    }

    /// <summary>
    /// Changes the category name if the new name is valid.
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
    /// Moves the category under another category, or removes the parent.
    /// </summary>
    public Result MoveTo(Guid? parentCategoryId)
    {
        var result = ValidateParentCategoryId(parentCategoryId);
        if (result.IsFailure)
            return result;

        if (parentCategoryId == Id)
        {
            return Result.Failure(
                new Error("ProductCategory.InvalidParentCategory", "A product category cannot be its own parent."));
        }

        ParentCategoryId = parentCategoryId;
        return Result.Success();
    }

    /// <summary>
    /// Checks that a category name is not empty and is not too long.
    /// </summary>
    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(
                new Error("ProductCategory.InvalidName", "Product category name is required."));
        }

        if (name.Trim().Length > 100)
        {
            return Result.Failure(
                new Error("ProductCategory.NameTooLong", "Product category name cannot be longer than 100 characters."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the parent category id is either empty or a real id value.
    /// </summary>
    private static Result ValidateParentCategoryId(Guid? parentCategoryId)
    {
        if (parentCategoryId == Guid.Empty)
        {
            return Result.Failure(
                new Error("ProductCategory.InvalidParentCategoryId", "Parent category id must be a valid id when provided."));
        }

        return Result.Success();
    }
}
