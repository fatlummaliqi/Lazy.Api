using Lazy.Api.Entities.Base;
using Lazy.Api.Infrastructure.Attributes;

namespace Lazy.Api.Entities;

[EntityRoute(Name = "todo-lists")]
public class TodoList : BaseEntity
{
    public string Name { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public virtual IList<Todo> Todos { get; set; } = [];
}
