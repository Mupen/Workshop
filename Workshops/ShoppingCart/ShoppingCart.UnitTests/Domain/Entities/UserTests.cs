using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.UnitTests.Domain.Entities;

public sealed class UserTests
{
    [Fact]
    public void Create_Should_create_active_user_when_values_are_valid()
    {
        var result = User.Create(
            " CUSTOMER@Example.COM ",
            "Demo Customer",
            "hashed-password",
            UserRole.Customer);

        Assert.True(result.IsSuccess);
        Assert.Equal("customer@example.com", result.Value.Email);
        Assert.Equal("Demo Customer", result.Value.DisplayName);
        Assert.Equal(UserRole.Customer, result.Value.Role);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Create_Should_fail_when_email_is_invalid()
    {
        var result = User.Create(
            "not-an-email",
            "Demo Customer",
            "hashed-password",
            UserRole.Customer);

        Assert.True(result.IsFailure);
        Assert.Equal("User.InvalidEmail", result.Error.Code);
    }

    [Fact]
    public void ChangeRole_Should_fail_when_role_is_invalid()
    {
        var user = User.Create(
            "customer@example.com",
            "Demo Customer",
            "hashed-password",
            UserRole.Customer).Value;

        var result = user.ChangeRole((UserRole)999);

        Assert.True(result.IsFailure);
        Assert.Equal("User.InvalidRole", result.Error.Code);
    }
}
