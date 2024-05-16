using Lazy.Api.Entities.Base;

namespace Lazy.Api.Infrastructure.Exceptions;

public class EntityUnprocessableException<T>() : Exception($"Entity '{typeof(T).Name}' is unprocessable.") where T : BaseEntity
{
}
