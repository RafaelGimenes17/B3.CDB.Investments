using Microsoft.OpenApi.Models;

namespace B3.CDB.WebApi.Configurations
{
    /// <summary>
    /// Configurações do Swagger/OpenAPI para documentação da API.
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Adiciona as configurações do Swagger ao contêiner de serviços.
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação.</param>
        /// <returns>A coleção de serviços atualizada.</returns>
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CDB Web API",
                    Version = "v1",
                    Description = "API para cálculo de investimentos em CDB",
                    Contact = new OpenApiContact
                    {
                        Name = "CDB Web API"
                    }
                });
            });

            return services;
        }
    }
}
