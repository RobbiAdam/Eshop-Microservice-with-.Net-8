namespace Ordering.Application.Extensions;
public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(orders => new OrderDto(
            Id: orders.Id.Value,
            CustomerId: orders.CustomerId.Value,
            OrderName: orders.OrderName.Value,
            ShippingAddress: new AddressDto(
                    orders.ShippingAddress.FirstName,
                    orders.ShippingAddress.LastName,
                    orders.ShippingAddress.EmailAddress,
                    orders.ShippingAddress.AddressLine,
                    orders.ShippingAddress.State,
                    orders.ShippingAddress.Country,
                    orders.ShippingAddress.ZipCode),
            BillingAddress: new AddressDto(
                    orders.BillingAddress.FirstName,
                    orders.BillingAddress.LastName,
                    orders.BillingAddress.EmailAddress,
                    orders.BillingAddress.AddressLine,
                    orders.BillingAddress.State,
                    orders.BillingAddress.Country,
                    orders.BillingAddress.ZipCode),
            Payment: new PaymentDto(
                    orders.Payment.CardName,
                    orders.Payment.CardNumber,
                    orders.Payment.Expiration,
                    orders.Payment.CVV,
                    orders.Payment.PaymentMethod),
            Status: orders.Status,
            OrderItems: orders.OrderItems.Select(
                    oi => new OrderItemDto(
                    oi.Id.Value,
                    oi.ProductId.Value,
                    oi.Quantity,
                    oi.Price)).ToList()
                                 ));
    }
}
