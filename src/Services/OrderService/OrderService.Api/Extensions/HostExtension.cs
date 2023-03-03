using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace OrderService.Api.Extensions
{
    public static class HostExtension
    {
        public static WebApplication MigrateDbContext<TContext>(this WebApplication host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Migration database associated with context {DbContextName} started", typeof(TContext).Name);

                // Retry policy for Sql Exception
                var retry = Policy.Handle<SqlException>()  
                                  .WaitAndRetry(new TimeSpan[]
                                  {
                                      TimeSpan.FromSeconds(3),
                                      TimeSpan.FromSeconds(5),
                                      TimeSpan.FromSeconds(8),
                                  });

                // Execution for Retry policy. 
                retry.Execute(() => InvokeSeeder(seeder, context, services));

                logger.LogInformation("Migrated databases");
            }
            catch (Exception ex)
            {
                logger.LogError("Order migrate db context failed on {DbContextName} !", typeof(TContext).Name);
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.EnsureCreated();

            context.Database.Migrate();

            seeder(context, services);
        }
    }
}
