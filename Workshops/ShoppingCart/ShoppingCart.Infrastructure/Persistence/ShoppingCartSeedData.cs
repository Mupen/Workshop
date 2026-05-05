using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Infrastructure.Persistence;

public static class ShoppingCartSeedData
{
    public static readonly Guid DemoUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    /// <summary>
    /// Adds sample brands, categories, products, and a cart for local development.
    /// </summary>
    public static async Task SeedDevelopmentDataAsync(ShoppingCartDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        await GetOrCreateUserAsync(
            dbContext,
            id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            email: "admin@shoppingcart.local",
            displayName: "Demo Admin",
            role: UserRole.Admin);

        await GetOrCreateUserAsync(
            dbContext,
            id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            email: "clerk@shoppingcart.local",
            displayName: "Demo Clerk",
            role: UserRole.Clerk);

        await GetOrCreateUserAsync(
            dbContext,
            id: DemoUserId,
            email: "customer@shoppingcart.local",
            displayName: "Demo Customer",
            role: UserRole.Customer);

        var keyCo = await GetOrCreateBrandAsync(dbContext, "KeyCo");
        var contoso = await GetOrCreateBrandAsync(dbContext, "Contoso");
        var northwind = await GetOrCreateBrandAsync(dbContext, "Northwind");

        var electronics = await GetOrCreateCategoryAsync(dbContext, "Electronics");
        var computers = await GetOrCreateCategoryAsync(dbContext, "Computers", electronics.Id);
        var laptops = await GetOrCreateCategoryAsync(dbContext, "Laptops", computers.Id);
        var books = await GetOrCreateCategoryAsync(dbContext, "Books");
        var digitalGoods = await GetOrCreateCategoryAsync(dbContext, "Digital Goods");

        var keyboard = await GetOrCreateProductAsync(
            dbContext,
            sku: "KEY-001",
            name: "Mechanical Keyboard",
            description: "Compact mechanical keyboard with tactile switches.",
            brandId: keyCo.Id,
            categoryId: electronics.Id,
            unitPrice: 899m,
            trackInventory: true,
            stockQuantity: 25);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "LAP-001",
            name: "Gaming Laptop",
            description: "Portable gaming laptop with dedicated graphics.",
            brandId: contoso.Id,
            categoryId: laptops.Id,
            unitPrice: 14999m,
            trackInventory: true,
            stockQuantity: 6);

        var ebook = await GetOrCreateProductAsync(
            dbContext,
            sku: "DIG-001",
            name: "Clean Architecture E-book",
            description: "Digital book about layered application architecture.",
            brandId: northwind.Id,
            categoryId: digitalGoods.Id,
            unitPrice: 199m,
            trackInventory: false,
            stockQuantity: null);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "BOOK-001",
            name: ".NET API Handbook",
            description: "Printed guide to building APIs with ASP.NET Core.",
            brandId: northwind.Id,
            categoryId: books.Id,
            unitPrice: 349m,
            trackInventory: true,
            stockQuantity: 12);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "MOU-001",
            name: "Wireless Mouse",
            description: "Ergonomic wireless mouse with adjustable DPI.",
            brandId: keyCo.Id,
            categoryId: electronics.Id,
            unitPrice: 299m,
            trackInventory: true,
            stockQuantity: 40);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "MON-001",
            name: "27 Inch Monitor",
            description: "QHD office monitor with height-adjustable stand.",
            brandId: contoso.Id,
            categoryId: electronics.Id,
            unitPrice: 2499m,
            trackInventory: true,
            stockQuantity: 10);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "USB-001",
            name: "USB-C Docking Station",
            description: "Multi-port USB-C dock with HDMI, Ethernet, and USB ports.",
            brandId: contoso.Id,
            categoryId: computers.Id,
            unitPrice: 1199m,
            trackInventory: true,
            stockQuantity: 15);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "BAG-001",
            name: "Laptop Backpack",
            description: "Water-resistant backpack with padded laptop compartment.",
            brandId: northwind.Id,
            categoryId: computers.Id,
            unitPrice: 699m,
            trackInventory: true,
            stockQuantity: 18);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "DIG-002",
            name: "C# Refactoring Checklist",
            description: "Downloadable checklist for improving C# codebases.",
            brandId: northwind.Id,
            categoryId: digitalGoods.Id,
            unitPrice: 99m,
            trackInventory: false,
            stockQuantity: null);

        await GetOrCreateProductAsync(
            dbContext,
            sku: "BOOK-002",
            name: "Entity Framework Core Guide",
            description: "Printed guide for database access with EF Core.",
            brandId: northwind.Id,
            categoryId: books.Id,
            unitPrice: 399m,
            trackInventory: true,
            stockQuantity: 9);

        await GetOrCreateDemoCartAsync(dbContext, keyboard, ebook);
    }

    /// <summary>
    /// Gets an existing user by email or creates it if it is missing.
    /// </summary>
    private static async Task<User> GetOrCreateUserAsync(
        ShoppingCartDbContext dbContext,
        Guid id,
        string email,
        string displayName,
        UserRole role)
    {
        var existing = await dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
        if (existing is not null)
            return existing;

        var user = User.Create(
            email,
            displayName,
            passwordHash: "development-password-hash",
            role,
            createdAtUtc: DateTime.Parse("2026-04-29T00:00:00Z").ToUniversalTime(),
            id).Value;

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Gets an existing brand by name or creates it if it is missing.
    /// </summary>
    private static async Task<ProductBrand> GetOrCreateBrandAsync(ShoppingCartDbContext dbContext, string name)
    {
        var existing = await dbContext.ProductBrands.SingleOrDefaultAsync(brand => brand.Name == name);
        if (existing is not null)
            return existing;

        var brand = ProductBrand.Create(name).Value;
        await dbContext.ProductBrands.AddAsync(brand);
        await dbContext.SaveChangesAsync();
        return brand;
    }

    /// <summary>
    /// Gets an existing category by name or creates it if it is missing.
    /// </summary>
    private static async Task<ProductCategory> GetOrCreateCategoryAsync(
        ShoppingCartDbContext dbContext,
        string name,
        Guid? parentCategoryId = null)
    {
        var existing = await dbContext.ProductCategories.SingleOrDefaultAsync(category => category.Name == name);
        if (existing is not null)
            return existing;

        var category = ProductCategory.Create(name, parentCategoryId).Value;
        await dbContext.ProductCategories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Gets an existing product by SKU or creates it if it is missing.
    /// </summary>
    private static async Task<Product> GetOrCreateProductAsync(
        ShoppingCartDbContext dbContext,
        string sku,
        string name,
        string description,
        Guid brandId,
        Guid categoryId,
        decimal unitPrice,
        bool trackInventory,
        int? stockQuantity)
    {
        var existing = await dbContext.Products.SingleOrDefaultAsync(product => product.Sku == sku);
        if (existing is not null)
            return existing;

        var product = Product.Create(
            sku,
            name,
            description,
            brandId,
            categoryId,
            unitPrice,
            trackInventory,
            stockQuantity).Value;

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Creates one demo cart with sample products if the demo user does not already have one.
    /// </summary>
    private static async Task GetOrCreateDemoCartAsync(
        ShoppingCartDbContext dbContext,
        Product keyboard,
        Product ebook)
    {
        var existing = await dbContext.ShoppingCarts
            .Include(cart => cart.Items)
            .AnyAsync(cart => cart.UserId == DemoUserId);

        if (existing)
            return;

        var cart = ShoppingCart.Domain.Entities.ShoppingCart.Create(DemoUserId).Value;
        cart.SetItem(keyboard, 1);
        cart.SetItem(ebook, 1);

        await dbContext.ShoppingCarts.AddAsync(cart);
        await dbContext.SaveChangesAsync();
    }
}
