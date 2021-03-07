using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using SagaPatternLibraryComparison.Domain.Orders.Events;

namespace SagaPatternLibraryComparison.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "EntryPointAPI", Version = "v1"}); });
            services.AddRebus(
                rebus => rebus
                   .Logging(l => l.Console())
                   .Routing(r => r.TypeBased().Map<OrderCreated>("OrdersProcessorQueue"))
                   .Transport(t => t.UseAzureServiceBusAsOneWayClient(_configuration.GetSection("AzureServiceBus:ConnectionString").Value))
                   .Options(t => t.SimpleRetryStrategy(errorQueueAddress: "ErrorQueue")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EntryPointAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
