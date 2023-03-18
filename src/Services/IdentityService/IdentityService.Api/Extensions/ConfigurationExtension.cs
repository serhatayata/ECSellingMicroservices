﻿namespace IdentityService.Api.Extensions
{
    public class ConfigurationExtension
    {
        static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static IConfiguration appConfig
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configurations/appsettings.json", optional: false) // if same value is used like ConnectionString, this will be the second option (optional)
                .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            }
        }

        public static IConfiguration serilogConfig
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configurations/serilog.json", optional: false) // if same value is used like ConnectionString, this will be the second option (optional)
                .AddJsonFile($"Configurations/serilog.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            }
        }
    }
}
