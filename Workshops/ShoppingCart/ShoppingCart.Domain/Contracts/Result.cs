namespace ShoppingCart.Domain.Contracts;

public class Result
{
    /// <summary>
    /// Stores whether an operation worked and, if it failed, why it failed.
    /// </summary>
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new ArgumentException("Success cannot have an error.");

        if (!isSuccess && error == Error.None)
            throw new ArgumentException("Failure must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    /// <summary>
    /// Creates a successful result for an operation that does not return a value.
    /// </summary>
    public static Result Success()
        => new(true, Error.None);

    /// <summary>
    /// Creates a failed result with an error explaining what went wrong.
    /// </summary>
    public static Result Failure(Error error)
        => new(false, error);

    /// <summary>
    /// Converts this failed result into a failed result that has a value type.
    /// </summary>
    public Result<T> ToFailure<T>()
        => Result<T>.Failure(Error);
}
