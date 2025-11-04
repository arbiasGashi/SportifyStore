using Catalog.API.Extensions;
using Catalog.Infrastructure.Data;

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
                            .AddApplicationServices();

            var app = builder.Build();

            // Seed database once at startup
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ICatalogContext>();
                await ((CatalogContext)context).SeedAsync();
            }

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
