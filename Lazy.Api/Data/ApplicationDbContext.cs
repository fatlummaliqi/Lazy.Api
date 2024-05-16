using Lazy.Api.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lazy.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = Assembly.GetExecutingAssembly()
                                  .GetExportedTypes()
                                  .Where(type => !type.IsAbstract &&
                                                 !type.IsInterface &&
                                                 type.IsSubclassOf(typeof(BaseEntity)));

        foreach (var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }
    }
}
