using ShoppingCart.Domain.Entities;

namespace ShoppingCart.UnitTests.Domain.Entities;

public sealed class ProductBrandTests
{
    [Fact]
    public void Create_Should_create_brand()
    {
        var result = ProductBrand.Create("KeyCo");

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal("KeyCo", result.Value.Name);
    }

    [Fact]
    public void Create_Should_fail_when_name_is_missing()
    {
        var result = ProductBrand.Create("");

        Assert.True(result.IsFailure);
        Assert.Equal("ProductBrand.InvalidName", result.Error.Code);
    }

    [Fact]
    public void Rename_Should_update_name()
    {
        var brand = ProductBrand.Create("Old Brand").Value;

        var result = brand.Rename("New Brand");

        Assert.True(result.IsSuccess);
        Assert.Equal("New Brand", brand.Name);
    }
}
