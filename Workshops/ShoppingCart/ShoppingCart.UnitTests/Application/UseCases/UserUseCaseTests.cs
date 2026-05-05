using ShoppingCart.Application.Requests.Users;
using ShoppingCart.Application.UseCases.Users;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;
using ShoppingCart.UnitTests.Helpers;

namespace ShoppingCart.UnitTests.Application.UseCases;

public sealed class UserUseCaseTests
{
    [Fact]
    public async Task RegisterCustomer_Should_create_customer_user()
    {
        var users = new FakeUserRepository();
        var useCase = new RegisterCustomer(users);
        var request = new RegisterCustomerRequest(
            "customer@example.com",
            "Demo Customer",
            "hashed-password");

        var result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Single(users.Users);
        Assert.Equal(UserRole.Customer, result.Value.Role);
    }

    [Fact]
    public async Task CreateUser_Should_fail_when_email_already_exists()
    {
        var users = new FakeUserRepository();
        await users.AddAsync(User.Create(
            "admin@example.com",
            "Demo Admin",
            "hashed-password",
            UserRole.Admin).Value);
        var useCase = new CreateUser(users);
        var request = new CreateUserRequest(
            "ADMIN@example.com",
            "Another Admin",
            "hashed-password",
            UserRole.Admin);

        var result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal("User.DuplicateEmail", result.Error.Code);
        Assert.Single(users.Users);
    }

    [Fact]
    public async Task ChangeUserRole_Should_change_role_when_user_exists()
    {
        var users = new FakeUserRepository();
        var user = User.Create(
            "clerk@example.com",
            "Demo Clerk",
            "hashed-password",
            UserRole.Clerk).Value;
        await users.AddAsync(user);
        var useCase = new ChangeUserRole(users);

        var result = await useCase.ExecuteAsync(new ChangeUserRoleRequest(user.Id, UserRole.Admin));

        Assert.True(result.IsSuccess);
        Assert.Equal(UserRole.Admin, user.Role);
    }
}
