using Lazy.Api.Entities.Base;
using Lazy.Api.Infrastructure.Attributes;

namespace Lazy.Api.Entities;

[IgnoreEntityResource]
[EntityRoute(Name = "users")]
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }

    public virtual IList<TodoList> TodoList { get; set; } = [];
}
