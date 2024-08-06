namespace Ordering.Application.Dtos;

public record OrderItemDto(
    Guid OrderId,
    Guid Productid,
    int Quantity,
    decimal Price);