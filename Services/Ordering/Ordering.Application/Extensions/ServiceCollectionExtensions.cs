using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System.Reflection;

namespace Ordering.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Pipeline behaviors (order matters)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        return services;
    }
}
