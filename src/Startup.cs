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
            services.AddAvailability();
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
            if (env.IsEnvironment("local"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<Availability>();
            app.UseMiddleware<ApiExceptionHandling>();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseHealthChecks("/healthcheck", HealthCheckConfig.Options);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"{(env.IsEnvironment("local") ? string.Empty : "/complimentscomplaintsservice")}/swagger/v1/swagger.json", "Compliments complaints service API");
            });
        }
    }
}
