namespace Ordering.Application.Orders.Commands.DeleteOrder;
public class DeleteOrderHandler(IApplicationDbContext context) 
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);

        var order = await context.Orders.FindAsync([orderId], cancellationToken: cancellationToken);

        if(order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }
        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
