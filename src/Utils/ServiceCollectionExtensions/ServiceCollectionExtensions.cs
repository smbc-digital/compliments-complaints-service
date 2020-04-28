using System.Collections.Generic;
using compliments_complaints_service.Config;
using compliments_complaints_service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StockportGovUK.AspNetCore.Gateways.MailingServiceGateway;
using StockportGovUK.AspNetCore.Gateways.VerintServiceGateway;

namespace compliments_complaints_service.Utils.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IComplimentsService, ComplimentsService>(provider => new ComplimentsService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IOptions<ComplimentsListConfiguration>>()));
            services.AddTransient<IComplaintsService, ComplaintsService>(provider => new ComplaintsService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IOptions<ComplaintsListConfiguration>>(),
                provider.GetService<IMailingServiceGateway>(),
                provider.GetService<ILogger<ComplaintsService>>()));
            services.AddTransient<IFeedbackService, FeedbackService>(provider => new FeedbackService(
                provider.GetService<IVerintServiceGateway>(),
                provider.GetService<IOptions<FeedbackListConfiguration>>()));
            services.AddSingleton<IVerintServiceGateway, VerintServiceGateway>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Compliments Complaints service API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Authorization using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
