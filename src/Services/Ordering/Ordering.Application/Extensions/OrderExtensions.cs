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
    public static OrderDto ToOrderDto(this Order order)
    {
        return DtoFromOrder(order);
    }

    private static OrderDto DtoFromOrder(Order order)
    {
        return new OrderDto(
                    Id: order.Id.Value,
                    CustomerId: order.CustomerId.Value,
                    OrderName: order.OrderName.Value,
                    ShippingAddress: new AddressDto(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.EmailAddress!, order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode),
                    BillingAddress: new AddressDto(order.BillingAddress.FirstName, order.BillingAddress.LastName, order.BillingAddress.EmailAddress!, order.BillingAddress.AddressLine, order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode),
                    Payment: new PaymentDto(order.Payment.CardName!, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.CVV, order.Payment.PaymentMethod),
                    Status: order.Status,
                    OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
                );
    }
}
