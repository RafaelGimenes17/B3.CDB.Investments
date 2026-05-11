using Microsoft.AspNetCore.Mvc;

namespace B3.CDB.WebApi.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
            });

            services.AddCors(options =>
            {
                var corsOrigins = configuration.GetSection("CorsOrigins").Get<string[]>() ?? ["http://localhost:3000"];

                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(corsOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHealthChecks();

            return services;
        }
    }
}