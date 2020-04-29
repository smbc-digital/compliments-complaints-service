using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockportGovUK.AspNetCore.Middleware;
using StockportGovUK.AspNetCore.Availability;
using StockportGovUK.AspNetCore.Availability.Middleware;
using compliments_complaints_service.Config;
using Microsoft.Extensions.Hosting;
using compliments_complaints_service.Utils.HealthChecks;
using compliments_complaints_service.Utils.ServiceCollectionExtensions;
using StockportGovUK.NetStandard.Gateways;

namespace compliments_complaints_service
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddResilientHttpClients<IGateway, Gateway>(Configuration);
            services.AddAvailability();
            services.AddSwagger();
            services.AddHealthChecks()
                .AddCheck<TestHealthCheck>("TestHealthCheck");

            services.Configure<FeedbackListConfiguration>(Configuration.GetSection("FeedbackConfiguration"));
            services.Configure<ComplimentsListConfiguration>(Configuration.GetSection("ComplimentsConfiguration"));
            services.Configure<ComplaintsListConfiguration>(Configuration.GetSection("ComplaintsConfiguration"));

            services.RegisterServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("local"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseMiddleware<Availability>();
            app.UseMiddleware<ExceptionHandling>();

            app.UseHealthChecks("/healthcheck", HealthCheckConfig.Options);
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"{(env.IsEnvironment("local") ? string.Empty : "/complimentscomplaintsservice")}/swagger/v1/swagger.json", "Compliments Complaints service API");
            });
        }
    }
}
