using Lazy.Api.Entities.Base;
using Lazy.Api.Infrastructure.Attributes;
using System.Reflection;

namespace Lazy.Api.Infrastructure;

public static class Resolver
{
    public static IEnumerable<Type> ResolveEntityTypes()
        => Assembly.GetExecutingAssembly()
                   .GetExportedTypes()
                   .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                                   type.IsSubclassOf(typeof(BaseEntity)));

    public static IEnumerable<Type> ResolveEndpointsGroups()
        => Assembly.GetExecutingAssembly()
                   .GetExportedTypes()
                   .Where(type => type is { IsGenericType: false, IsAbstract: false, IsInterface: false } &&
                                  typeof(IEndpointsGroup).IsAssignableFrom(type));

    public static string ResolveRouteFromType(Type type) 
        => type.GetCustomAttribute<EntityRouteAttribute>() is EntityRouteAttribute entityRouteAttribute
                ? entityRouteAttribute.Name
                : type.Name.ToLowerInvariant();
}
