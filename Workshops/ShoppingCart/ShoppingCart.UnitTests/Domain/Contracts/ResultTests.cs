using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.UnitTests.Domain.Contracts;

public sealed class ResultTests
{
    [Fact]
    public void Success_Should_not_have_error()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_Should_have_error()
    {
        var error = new Error("Test.Error", "Test failure.");

        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_value_Should_not_allow_value_access()
    {
        var result = Result<string>.Failure(new Error("Test.Error", "Test failure."));

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
