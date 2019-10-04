﻿using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockportGovUK.AspNetCore.Middleware;
using StockportGovUK.AspNetCore.Availability;
using StockportGovUK.AspNetCore.Availability.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using compliments_complaints_service.Services;
using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;
using StockportGovUK.AspNetCore.Gateways;
using compliments_complaints_service.Config;
using compliments_complaints_service.Utils;
using Microsoft.Extensions.Logging;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "compliments_complaints_service API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Authorization using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                });
            });

            services.AddHttpClient();

            services.AddAvailability();
            services.AddSingleton<IEventCodesHelper, EventCodesHelper>();
            services.AddTransient<IComplimentsService, ComplimentsService>(provider => new ComplimentsService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IEventCodesHelper>(),
                provider.GetService<ILogger>()));
            services.AddTransient<IComplaintsService, ComplaintsService>(provider => new ComplaintsService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IEventCodesHelper>()));
            services.AddTransient<IFeedbackService, FeedbackService>(provider => new FeedbackService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IEventCodesHelper>()));
            services.AddSingleton<IVerintServiceGateway, VerintServiceGateway>();

            services.AddResilientHttpClients<IGateway, Gateway>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseMiddleware<Availability>();
            app.UseMiddleware<ExceptionHandling>();
            app.UseHttpsRedirection();
            app.UseSwagger();

            var swaggerPrefix = env.IsDevelopment() ? string.Empty : "/complimentscomplaintsservice";
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{swaggerPrefix}/swagger/v1/swagger.json", "compliments_complaints_service API");
            });
            app.UseMvc();
        }
    }
}
