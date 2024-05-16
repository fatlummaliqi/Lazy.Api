using Lazy.Api.Data;
using Lazy.Api.Entities;
using Lazy.Api.Infrastructure;
using Lazy.Api.Infrastructure.Attributes;
using Lazy.Api.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Lazy.Api.Endpoints;

[EntityRoute(Name = "users")]
public class Users : IEndpointsGroup
{
    public void Map(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("register", Register).WithName(Guid.NewGuid().ToString());
        routeGroupBuilder.MapGet(string.Empty, GetByEmail).WithName(Guid.NewGuid().ToString());
    }

    public async Task<User> Register(ApplicationDbContext dbContext, string name, string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            throw new EntityUnprocessableException<User>(); 
        }

        var user = new User
        {
            Name = name,
            Email = email
        };

        var insertedUserEntry = await dbContext.Set<User>().AddAsync(user, cancellationToken);

        var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);

        if (affectedRows > 0)
        {
            return insertedUserEntry.Entity;
        }

        throw new EntityNotPersistedException<User>();
    }

    public async Task<User> GetByEmail(ApplicationDbContext dbContext, string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new EntityNotFoundException<User>(requestedProperty: nameof(User.Email), requestedValue: email);
        }

        var user = await dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (user == null)
        {
            throw new EntityNotFoundException<User>(requestedProperty: nameof(User.Email), requestedValue: email);
        }

        return user;
    }
}
