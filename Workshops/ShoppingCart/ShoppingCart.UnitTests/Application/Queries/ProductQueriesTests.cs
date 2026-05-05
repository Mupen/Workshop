using ShoppingCart.Application.Queries.Products;
using ShoppingCart.Domain.Entities;
using ShoppingCart.UnitTests.Helpers;

namespace ShoppingCart.UnitTests.Application.Queries;

public sealed class ProductQueriesTests
{
    [Fact]
    public async Task GetAllProducts_Should_include_brand_and_category_names()
    {
        var context = await CreateContextAsync();
        var query = new GetAllProducts(context.Products, context.Brands, context.Categories);

        var products = await query.ExecuteAsync();

        var product = Assert.Single(products);
        Assert.Equal("KeyCo", product.BrandName);
        Assert.Equal("Electronics", product.CategoryName);
    }

    [Fact]
    public async Task GetAvailableProducts_Should_only_return_products_that_can_be_bought()
    {
        var context = await CreateContextAsync();
        var unavailable = Product.Create(
            "SKU-002",
            "Sold out mouse",
            "No stock left.",
            context.Brand.Id,
            context.Category.Id,
            50m,
            trackInventory: true,
            stockQuantity: 0).Value;
        await context.Products.AddAsync(unavailable);
        var query = new GetAvailableProducts(context.Products, context.Brands, context.Categories);

        var products = await query.ExecuteAsync();

        var product = Assert.Single(products);
        Assert.Equal("SKU-001", product.Sku);
        Assert.True(product.IsAvailable);
    }

    [Fact]
    public async Task GetProductById_Should_return_null_when_product_does_not_exist()
    {
        var context = await CreateContextAsync();
        var query = new GetProductById(context.Products, context.Brands, context.Categories);

        var product = await query.ExecuteAsync(Guid.NewGuid());

        Assert.Null(product);
    }

    private static async Task<TestContext> CreateContextAsync()
    {
        var products = new FakeProductRepository();
        var brands = new FakeProductBrandRepository();
        var categories = new FakeProductCategoryRepository();
        var brand = ProductBrand.Create("KeyCo").Value;
        var category = ProductCategory.Create("Electronics").Value;
        var product = Product.Create(
            "SKU-001",
            "Keyboard",
            "Mechanical keyboard",
            brand.Id,
            category.Id,
            100m,
            trackInventory: true,
            stockQuantity: 10).Value;

        await brands.AddAsync(brand);
        await categories.AddAsync(category);
        await products.AddAsync(product);

        return new TestContext(products, brands, categories, brand, category, product);
    }

    private sealed record TestContext(
        FakeProductRepository Products,
        FakeProductBrandRepository Brands,
        FakeProductCategoryRepository Categories,
        ProductBrand Brand,
        ProductCategory Category,
        Product Product);
}
