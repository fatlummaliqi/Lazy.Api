using Lazy.Api.Entities.Base;

namespace Lazy.Api.Infrastructure.Exceptions;

public class EntityNotFoundException<T>(string requestedProperty, object requestedValue) 
    : Exception($"Entity '{typeof(T).Name}' with requested {requestedProperty}='{requestedValue}' was not found.") where T : BaseEntity
{
}
