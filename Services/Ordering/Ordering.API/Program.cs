using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API;

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            await app.Services.ApplyOrderingMigrationAsync();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
