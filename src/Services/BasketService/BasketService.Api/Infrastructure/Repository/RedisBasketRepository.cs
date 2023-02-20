﻿using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace BasketService.Api.Infrastructure.Repository
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
         
        public RedisBasketRepository(ILogger<RedisBasketRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
        }

        #region DeleteBasketAsync
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
        #endregion
        #region UpdateBasketAsync
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));

            if (!created)
            {
                _logger.LogInformation("Problem occured persisting the item");
                return null;
            }

            _logger.LogInformation("Basket item persisted successfully");

            return await GetBasketAsync(basket.BuyerId);
        }
        #endregion
        #region GetBasketAsync
        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await _database.StringGetAsync(customerId);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(data);
        }
        #endregion
        #region GetUsers
        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }
        #endregion

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();

            return _redis.GetServer(endpoint.First());
        }
    }
}
