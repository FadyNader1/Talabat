using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Interfaces;

namespace Talabat.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase db;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }
        public async Task<CustomerBasket> CreateOrUpdateAsyc(CustomerBasket item)
        {

            var basket = db.StringSetAsync(item.Id, JsonSerializer.Serialize(item), TimeSpan.FromDays(1));
            if (basket is null)
                return null;
            return await GetBasketByIdAsync(item.Id);

        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await db.KeyDeleteAsync(id);
        }

        public Task<IReadOnlyList<CustomerBasket>> GetAllBasketAsync()
        {
            //var baskets = db.Multiplexer.GetServer(db.Multiplexer.GetEndPoints()[0]).Keys(pattern: "*").ToList();
            //var customerBaskets = new List<CustomerBasket>();
            //foreach (var basket in baskets)
            //{
            //    var basketData = db.StringGet(basket);
            //    if (basketData.HasValue)
            //    {
            //        var customerBasket = JsonSerializer.Deserialize<CustomerBasket>(basketData);
            //        if (customerBasket != null)
            //        {
            //            customerBaskets.Add(customerBasket);
            //        }
            //    }
            //}   
            //return Task.FromResult<IReadOnlyList<CustomerBasket>>(customerBaskets);
            var server = db.Multiplexer.GetServer(db.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(pattern: "*");

            //var baskets = keys
            //    .Select(k => db.StringGet(k))
            //    .Where(val => val.HasValue)
            //    .Select(val => JsonSerializer.Deserialize<CustomerBasket>(val))
            //    .Where(b => b != null)
            //    .ToList();
            var baskets = keys.Select(x => db.StringGet(x))
                .Where(y => y.HasValue)
                .Select(y => JsonSerializer.Deserialize<CustomerBasket>(y))
                .Where(z => z != null)
                .ToList();

            return Task.FromResult<IReadOnlyList<CustomerBasket>>(baskets!);
        }

        public async Task<CustomerBasket> GetBasketByIdAsync(string id)
        {
            var basket = await db.StringGetAsync(id);
            if (!basket.HasValue)
                return null;
            
            return  JsonSerializer.Deserialize<CustomerBasket>(basket);

        }
        public async Task<bool> ClearBasketAsync()
        {
            var baskets = await GetAllBasketAsync();
            if (baskets is null || !baskets.Any())
                return false;
            foreach (var x in baskets)
            {
                db.KeyDelete(x.Id);

            }
            return true;
        }
    }
}
