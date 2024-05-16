using Lazy.Api.Data;
using Lazy.Api.Entities.Base;
using Lazy.Api.Infrastructure;
using Lazy.Api.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Lazy.Api.Endpoints;

public class EntityEndpoints<T> : IEndpointsGroup where T : BaseEntity
{
    public void Map(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("{id}", GetById).WithName(Guid.NewGuid().ToString());
        routeGroupBuilder.MapPost(string.Empty, Create).WithName(Guid.NewGuid().ToString());
        routeGroupBuilder.MapPut("{id}", Update).WithName(Guid.NewGuid().ToString());
        routeGroupBuilder.MapDelete("{id}", Delete).WithName(Guid.NewGuid().ToString());
        routeGroupBuilder.MapGet(string.Empty, GetPaginated).WithName(Guid.NewGuid().ToString());
    }

    public async Task<T> GetById(ApplicationDbContext dbContext, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new EntityNotFoundException<T>(requestedProperty: nameof(BaseEntity.Id), requestedValue: id);
        }

        var entity = await dbContext.Set<T>().FindAsync(id,cancellationToken) ?? throw new EntityNotFoundException<T>(requestedProperty: nameof(BaseEntity.Id), requestedValue: id);

        return entity;
    }

    public async Task<T> Create(ApplicationDbContext dbContext, T entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new EntityUnprocessableException<T>();
        }

        var insertedEntry = await dbContext.Set<T>().AddAsync(entity,cancellationToken);

        var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);

        if (affectedRows > 0)
        {
            return insertedEntry.Entity;
        }

        throw new EntityNotPersistedException<T>();
    }

    public async Task<T> Update(ApplicationDbContext dbContext, int id, T entity, CancellationToken cancellationToken)
    {
        if (id <= 0 || entity == null)
        {
            throw new EntityUnprocessableException<T>();
        }

        entity.Id = id;

        var updatedEntry = dbContext.Set<T>().Update(entity);

        var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);

        if (affectedRows > 0)
        {
            return updatedEntry.Entity;
        }

        throw new EntityNotPersistedException<T>();
    }

    public async Task<T> Delete(ApplicationDbContext dbContext, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new EntityNotFoundException<T>(requestedProperty: nameof(BaseEntity.Id), requestedValue: id);
        }

        var entity = await dbContext.Set<T>().FindAsync(id, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException<T>(requestedProperty: nameof(BaseEntity.Id), requestedValue: id);
        }

        var deletedEntry = dbContext.Set<T>().Remove(entity);

        var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);   

        if (affectedRows > 0)
        {
            return deletedEntry.Entity;
        }

        throw new EntityNotPersistedException<T>();
    }

    public async Task<IEnumerable<T>> GetPaginated(ApplicationDbContext dbContext, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        pageIndex = Math.Max(pageIndex, 0);
        pageSize = pageSize > 100 ? 100 : Math.Max(pageSize, 10);

        var entities = await dbContext.Set<T>()
                                      .OrderByDescending(entity => entity.Id)
                                      .Skip(pageIndex * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync(cancellationToken);

        return entities;
    }
}
