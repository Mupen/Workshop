namespace ShoppingCart.Application.Requests.Products;

public sealed record AdjustProductStockRequest(Guid ProductId, int Quantity);
