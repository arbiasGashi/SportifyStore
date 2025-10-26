
using Asp.Versioning;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Sorting;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add API Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog.API", Version = "v1" }); });

            /* Register AutoMapper
             * 
             * Scan the Catalog.Application assembly, because that’s where ProductMappingProfile is defined,
             * and automatically register all AutoMapper mapping profiles found there.
             * 
             * This allows us to inject IMapper anywhere in the application and use
             * predefined mapping configurations between domain entities and DTOs.
             */
            builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

            /* Register MediatR
             * 
             * Scan the Catalog.Application assembly, because that’s where GetAllBrandsQuery is defined,
             * and register all MediatR handlers (queries, commands, and notifications) found there.
             * 
             * This enables the CQRS pattern by letting MediatR automatically discover
             * and wire up request handlers defined in the Application layer.
             */
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllBrandsQuery).Assembly));

            // Register Application Services
            builder.Services.AddScoped<ICatalogContext, CatalogContext>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ITypeRepository, TypeRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();

            // Sorting strategies
            builder.Services.AddScoped<ISortStrategy, PriceAscSortStrategy>();
            builder.Services.AddScoped<ISortStrategy, PriceDescSortStrategy>();
            builder.Services.AddScoped<ISortStrategy, NameSortStrategy>();

            // Sorting factory
            builder.Services.AddScoped<ISortStrategyFactory, SortStrategyFactory>();

            var app = builder.Build();

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
