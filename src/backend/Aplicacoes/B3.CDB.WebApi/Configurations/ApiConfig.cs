using Microsoft.AspNetCore.Mvc;

namespace B3.CDB.WebApi.Configurations
{
    /// <summary>
    /// Configurações gerais da API, incluindo CORS e comportamento do modelo.
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// Adiciona as configurações da API ao contêiner de serviços.
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação.</param>
        /// <param name="configuration">Configurações da aplicação.</param>
        /// <returns>A coleção de serviços atualizada.</returns>
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
