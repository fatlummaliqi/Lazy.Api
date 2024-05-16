using Lazy.Api.Entities.Base;

namespace Lazy.Api.Infrastructure.Exceptions;

public class EntityNotPersistedException<T>() : Exception($"Entity '{typeof(T).Name}' was not persisted.") where T : BaseEntity
{
}
