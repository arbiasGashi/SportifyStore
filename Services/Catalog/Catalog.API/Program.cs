using Catalog.API.Extensions;
using Catalog.Infrastructure.Extensions;

namespace Catalog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add core services
            builder.Services.AddControllers();

            // Modular registrations
            builder.Services.AddApiVersioningConfig()
                            .AddSwaggerConfig()
                            .AddApplicationServices()
                            .AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Seed database once at startup
            await app.Services.EnsureCatalogDatabaseSeededAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
