using Lazy.Api.Data;
using Lazy.Api.Endpoints;
using Lazy.Api.Infrastructure;
using Lazy.Api.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lazy.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder MapEndpoints(this WebApplication builder)
    {
        MapSpecificEndpoints(builder);

        MapGenericEndpoints(builder);

        return builder;
    }

    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }

        return builder;
    }

    private static IApplicationBuilder MapSpecificEndpoints(WebApplication builder)
    {
        foreach (var group in Resolver.ResolveEndpointsGroups())
        {
            var resourceRoute = Resolver.ResolveRouteFromType(group);

            var routeGroupBuilder = builder.MapGroup(resourceRoute)
                                           .WithName(group.Name)
                                           .WithTags(group.Name);

            var entityEndpointsInstance = Activator.CreateInstance(group) as IEndpointsGroup;

            entityEndpointsInstance.Map(routeGroupBuilder);
        }

        return builder;
    }

    private static IApplicationBuilder MapGenericEndpoints(WebApplication builder)
    {
        var entityTypes = Resolver.ResolveEntityTypes().Where(type => type.GetCustomAttribute<IgnoreEntityResourceAttribute>() == null);

        foreach (var entityType in entityTypes)
        {
            var resourceRoute = Resolver.ResolveRouteFromType(entityType);

            var routeGroupBuilder = builder.MapGroup(resourceRoute)
                                           .WithName(entityType.Name)
                                           .WithTags(entityType.Name);

            var entityEndpointsType = typeof(EntityEndpoints<>).MakeGenericType(entityType);

            var entityEndpointsInstance = Activator.CreateInstance(entityEndpointsType) as IEndpointsGroup;

            entityEndpointsInstance.Map(routeGroupBuilder);
        }

        return builder;
    }
}
