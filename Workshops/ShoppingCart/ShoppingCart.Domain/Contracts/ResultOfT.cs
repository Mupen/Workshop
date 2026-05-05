namespace ShoppingCart.Domain.Contracts;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    /// <summary>
    /// Creates a successful result that contains a value.
    /// </summary>
    private Result(T value)
        : base(true, Error.None)
    {
        _value = value;
    }

    /// <summary>
    /// Creates a failed result that contains an error instead of a value.
    /// </summary>
    private Result(Error error)
        : base(false, error)
    {
        _value = default;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failure result.");

    /// <summary>
    /// Creates a successful result with the value the caller needs.
    /// </summary>
    public static Result<T> Success(T value)
        => new(value);

    /// <summary>
    /// Creates a failed result with an error explaining what went wrong.
    /// </summary>
    public static new Result<T> Failure(Error error)
        => new(error);
}
