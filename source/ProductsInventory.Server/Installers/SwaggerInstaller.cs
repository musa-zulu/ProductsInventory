using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ProductsInventory.Server.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IConfigurationRoot configRoot)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product Inventory System API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Email = "zuluchs@gmail.com",
                        Name = "Musa Zulu"                        
                    },
                    Description = "Developed by Musa Zulu, please click on the link below to contact him via email"
                });

                x.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}