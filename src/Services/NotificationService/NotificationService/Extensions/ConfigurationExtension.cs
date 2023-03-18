using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Extensions
{
    public class ConfigurationExtension
    {
        public static IConfiguration serilogConfig
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configurations/serilog.json")
                .AddEnvironmentVariables()
                .Build();
            }
        }
    }
}
