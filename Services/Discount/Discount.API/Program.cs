
using Discount.API.Extensions;
using Discount.API.Services;
using Discount.Infrastructure.Extensions;

namespace Discount.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Service registrations
        builder.Services
            .AddApplicationServices()
            .AddGrpcServices();

        var app = builder.Build();

        // Infrastructure startup
        app.InitializeDiscountDatabase<Program>();

        // HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapGrpcService<DiscountService>();

        app.MapGet("/", () =>
            "Communication with gRPC endpoints must be made through a gRPC client.");

        app.Run();
    }
}
