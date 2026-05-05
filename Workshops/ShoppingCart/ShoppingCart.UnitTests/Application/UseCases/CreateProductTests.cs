using ShoppingCart.Application.Requests.Products;
using ShoppingCart.Application.UseCases.Products;
using ShoppingCart.Domain.Entities;
using ShoppingCart.UnitTests.Helpers;

namespace ShoppingCart.UnitTests.Application.UseCases;

public sealed class CreateProductTests
{
    [Fact]
    public async Task ExecuteAsync_Should_create_product_when_brand_and_category_exist()
    {
        var context = await CreateContextAsync();
        var useCase = new CreateProduct(context.Products, context.Brands, context.Categories);
        var request = new CreateProductRequest(
            "SKU-001",
            "Keyboard",
            "Mechanical keyboard",
            context.Brand.Id,
            context.Category.Id,
            100m,
            TrackInventory: true,
            StockQuantity: 10);

        var result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Single(context.Products.Products);
        Assert.Equal(context.Brand.Id, result.Value.BrandId);
        Assert.Equal(context.Category.Id, result.Value.CategoryId);
    }

    [Fact]
    public async Task ExecuteAsync_Should_fail_when_brand_does_not_exist()
    {
        var context = await CreateContextAsync();
        var useCase = new CreateProduct(context.Products, context.Brands, context.Categories);
        var request = new CreateProductRequest(
            "SKU-001",
            "Keyboard",
            "Mechanical keyboard",
            Guid.NewGuid(),
            context.Category.Id,
            100m,
            TrackInventory: true,
            StockQuantity: 10);

        var result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal("ProductBrand.NotFound", result.Error.Code);
        Assert.Empty(context.Products.Products);
    }

    [Fact]
    public async Task ExecuteAsync_Should_fail_when_sku_already_exists()
    {
        var context = await CreateContextAsync();
        var useCase = new CreateProduct(context.Products, context.Brands, context.Categories);
        var request = new CreateProductRequest(
            "SKU-001",
            "Keyboard",
            "Mechanical keyboard",
            context.Brand.Id,
            context.Category.Id,
            100m,
            TrackInventory: true,
            StockQuantity: 10);
        await useCase.ExecuteAsync(request);

        var result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal("Product.DuplicateSku", result.Error.Code);
        Assert.Single(context.Products.Products);
    }

    private static async Task<TestContext> CreateContextAsync()
    {
        var products = new FakeProductRepository();
        var brands = new FakeProductBrandRepository();
        var categories = new FakeProductCategoryRepository();
        var brand = ProductBrand.Create("KeyCo").Value;
        var category = ProductCategory.Create("Electronics").Value;

        await brands.AddAsync(brand);
        await categories.AddAsync(category);

        return new TestContext(products, brands, categories, brand, category);
    }

    private sealed record TestContext(
        FakeProductRepository Products,
        FakeProductBrandRepository Brands,
        FakeProductCategoryRepository Categories,
        ProductBrand Brand,
        ProductCategory Category);
}
