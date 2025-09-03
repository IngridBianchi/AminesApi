using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary.Data;
using Oracle.EntityFrameworkCore;
using AddAdult.Data;
using Microsoft.EntityFrameworkCore;

namespace AddAdult
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseOracle(configuration.GetConnectionString("DefaultConnection"));
                    });
                    services.AddSingleton<ServiceBus.ISubscriptionReceiver, ServiceBus.SubscriptionReceiver>();
                    services.AddHostedService<Worker>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}