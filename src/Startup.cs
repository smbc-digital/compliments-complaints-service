using System.Diagnostics.CodeAnalysis;
using compliments_complaints_service.Config;
using compliments_complaints_service.Utils.HealthChecks;
using compliments_complaints_service.Utils.ServiceCollectionExtensions;
using StockportGovUK.AspNetCore.Availability;
using StockportGovUK.AspNetCore.Availability.Middleware;
using StockportGovUK.AspNetCore.Middleware;

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
            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddGateways(Configuration);
            services.RegisterServices();
            services.AddSwagger();

            services
                .AddHealthChecks()
                .AddCheck<TestHealthCheck>("TestHealthCheck");

            services.Configure<FeedbackListConfiguration>(Configuration.GetSection("FeedbackConfiguration"));
            services.Configure<ComplimentsListConfiguration>(Configuration.GetSection("ComplimentsConfiguration"));
            services.Configure<ComplaintsListConfiguration>(Configuration.GetSection("ComplaintsConfiguration"));

            services.RegisterServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler($"/api/v1/error{(env.IsDevelopment() ? "/local" : string.Empty)}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseHealthChecks("/healthcheck", HealthCheckConfig.Options);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Compliments and Complaints API");
            });
        }
    }
}
