using Lazy.Api.Entities.Base;
using Lazy.Api.Infrastructure.Attributes;

namespace Lazy.Api.Entities;

[EntityRoute(Name = "todos")]
public class Todo : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public int TodoListId { get; set; }

    public virtual TodoList TodoList { get; set; }
}
