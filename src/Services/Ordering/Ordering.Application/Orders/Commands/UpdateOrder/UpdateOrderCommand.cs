namespace Ordering.Application.Orders.Commands.UpdateOrder;
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order names cannot be empty");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer Id cannot be empty");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order items cannot be empty");
    }
}
