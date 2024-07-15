namespace Basket.API.Features.Basket.GetBasket
{
    public record class GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

    public record class GetBasketResult(ShoppingCart Cart);
    internal class GetBasketQueryHandler(
        IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(query.UserName);
            return new GetBasketResult(basket);
        }
    }
}
