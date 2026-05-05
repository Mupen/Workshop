using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Domain.Contracts;

namespace ShoppingCart.Api.Mapping;

internal static class HttpResultMapper
{
    /// <summary>
    /// Converts a failed domain/application result into an HTTP ProblemDetails response.
    /// </summary>
    public static IActionResult ToProblem(this ControllerBase controller, Result result)
    {
        var statusCode = GetStatusCode(result.Error.Code);

        return controller.Problem(
            title: "Request failed",
            detail: result.Error.Message,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = result.Error.Code
            });
    }

    /// <summary>
    /// Chooses the HTTP status code that best matches the application error code.
    /// </summary>
    private static int GetStatusCode(string errorCode)
    {
        if (errorCode.EndsWith(".NotFound", StringComparison.OrdinalIgnoreCase) ||
            errorCode.EndsWith("NotFound", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCodes.Status404NotFound;
        }

        if (errorCode.Contains("Duplicate", StringComparison.OrdinalIgnoreCase) ||
            errorCode.Contains("Conflict", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCodes.Status409Conflict;
        }

        return StatusCodes.Status400BadRequest;
    }
}
