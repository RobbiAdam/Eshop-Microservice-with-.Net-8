using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(
        IBasketRepository repository, IDistributedCache cache) 
        : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken ct = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, ct);
            if(!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket =  await repository.GetBasket(userName, ct);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), ct);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default)
        {
            await repository.StoreBasket(basket, ct);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), ct);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken ct = default)
        {
            await repository.DeleteBasket(userName, ct);
            await cache.RemoveAsync(userName, ct);
            return true;
        }


    }
}
