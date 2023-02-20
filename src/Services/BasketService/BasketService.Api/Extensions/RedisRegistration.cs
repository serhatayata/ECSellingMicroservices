using StackExchange.Redis;

namespace BasketService.Api.Extensions
{
    public static class RedisRegistration
    {
        public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider services, IConfiguration configuration)
        {
            var redisConf = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConf.ResolveDns = false;

            return ConnectionMultiplexer.Connect(redisConf);
        }
    }
}
