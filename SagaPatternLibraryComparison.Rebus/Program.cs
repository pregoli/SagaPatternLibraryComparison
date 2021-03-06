using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rebus.Auditing.Messages;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using SagaPatternLibraryComparison.Domain.Orders.Events;

namespace SagaPatternLibraryComparison.Rebus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => {

                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    services.AddRebus(
                        configure => configure
                           .Logging(l => l.Console())
                           .Routing(r => r.TypeBased().MapAssemblyOf<OrderCreated>("OrdersProcessorQueue"))
                           .Transport(t => t.UseAzureServiceBus(configuration.GetSection("AzureServiceBus:ConnectionString").Value, "OrdersProcessorQueue").AutomaticallyRenewPeekLock())
                           .Options(t => t.SimpleRetryStrategy(errorQueueAddress: "ErrorQueue"))
                           .Options(t => t.EnableMessageAuditing(auditQueue: "AuditQueue"))
                           .Sagas(s => s.StoreInSqlServer(
                                connectionString: configuration.GetSection("SqlServer:ConnectionString").Value,
                                dataTableName: "OrderProcessDemo",
                                indexTableName: "OrderProcessDemoIndex"))
                    );

                    services.AddHostedService<OrderWorkerService>();
                });
    }
}
