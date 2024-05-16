namespace Lazy.Api.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EntityRouteAttribute : Attribute
{
    public string Name { get; set; }
}
