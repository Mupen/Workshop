using ShoppingCart.Domain.Entities;

namespace ShoppingCart.UnitTests.Domain.Entities;

public sealed class ProductCategoryTests
{
    [Fact]
    public void Create_Should_create_root_category()
    {
        var result = ProductCategory.Create("Electronics");

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal("Electronics", result.Value.Name);
        Assert.Null(result.Value.ParentCategoryId);
        Assert.True(result.Value.IsRootCategory);
    }

    [Fact]
    public void Create_Should_create_child_category()
    {
        var parentCategoryId = Guid.NewGuid();

        var result = ProductCategory.Create("Laptops", parentCategoryId);

        Assert.True(result.IsSuccess);
        Assert.Equal(parentCategoryId, result.Value.ParentCategoryId);
        Assert.False(result.Value.IsRootCategory);
    }

    [Fact]
    public void Create_Should_fail_when_name_is_missing()
    {
        var result = ProductCategory.Create("");

        Assert.True(result.IsFailure);
        Assert.Equal("ProductCategory.InvalidName", result.Error.Code);
    }

    [Fact]
    public void MoveTo_Should_fail_when_category_is_its_own_parent()
    {
        var category = ProductCategory.Create("Laptops").Value;

        var result = category.MoveTo(category.Id);

        Assert.True(result.IsFailure);
        Assert.Equal("ProductCategory.InvalidParentCategory", result.Error.Code);
    }
}
