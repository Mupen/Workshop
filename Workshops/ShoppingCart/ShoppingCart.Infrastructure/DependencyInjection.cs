using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Infrastructure.Persistence;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers database access and repository classes for the infrastructure layer.
    /// </summary>
    public static IServiceCollection AddShoppingCartInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<ShoppingCartDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IProductBrandRepository, EfProductBrandRepository>();
        services.AddScoped<IProductCategoryRepository, EfProductCategoryRepository>();
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IShoppingCartRepository, EfShoppingCartRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();

        return services;
    }
}
