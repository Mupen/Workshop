using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Queries.ProductBrands;
using ShoppingCart.Application.Queries.ProductCategories;
using ShoppingCart.Application.Queries.Products;
using ShoppingCart.Application.Queries.ShoppingCarts;
using ShoppingCart.Application.Queries.Users;
using ShoppingCart.Application.UseCases.ProductBrands;
using ShoppingCart.Application.UseCases.ProductCategories;
using ShoppingCart.Application.UseCases.Products;
using ShoppingCart.Application.UseCases.ShoppingCarts;
using ShoppingCart.Application.UseCases.Users;

namespace ShoppingCart.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all application queries and use cases so controllers can ask for them.
    /// </summary>
    public static IServiceCollection AddShoppingCartApplication(this IServiceCollection services)
    {
        services.AddScoped<GetAllProductBrands>();
        services.AddScoped<GetProductBrandById>();
        services.AddScoped<CreateProductBrand>();
        services.AddScoped<RenameProductBrand>();

        services.AddScoped<GetAllProductCategories>();
        services.AddScoped<GetProductCategoryById>();
        services.AddScoped<CreateProductCategory>();
        services.AddScoped<RenameProductCategory>();
        services.AddScoped<MoveProductCategory>();

        services.AddScoped<GetAllProducts>();
        services.AddScoped<GetAvailableProducts>();
        services.AddScoped<GetProductById>();
        services.AddScoped<CreateProduct>();
        services.AddScoped<UpdateProductDetails>();
        services.AddScoped<ChangeProductBrand>();
        services.AddScoped<ChangeProductCategory>();
        services.AddScoped<ChangeProductPrice>();
        services.AddScoped<ChangeProductStatus>();
        services.AddScoped<IncreaseProductStock>();
        services.AddScoped<DecreaseProductStock>();

        services.AddScoped<GetAllShoppingCarts>();
        services.AddScoped<GetShoppingCartById>();
        services.AddScoped<GetShoppingCartsByUserId>();
        services.AddScoped<CreateShoppingCart>();
        services.AddScoped<SetShoppingCartItem>();
        services.AddScoped<DeleteShoppingCart>();

        services.AddScoped<GetAllUsers>();
        services.AddScoped<GetUserById>();
        services.AddScoped<GetUserByEmail>();
        services.AddScoped<CreateUser>();
        services.AddScoped<RegisterCustomer>();
        services.AddScoped<ChangeUserRole>();
        services.AddScoped<ActivateUser>();
        services.AddScoped<DeactivateUser>();

        return services;
    }
}
