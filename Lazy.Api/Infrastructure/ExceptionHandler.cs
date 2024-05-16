using Lazy.Api.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Lazy.Api.Infrastructure;

public class ExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<string, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public ExceptionHandler()
    {
        _exceptionHandlers = new()
            {
                { typeof(EntityNotFoundException<>).Name, HandleEntityNotFoundException },
                { typeof(EntityUnprocessableException<>).Name, HandleEntityUnprocessableException },
                { typeof(EntityNotPersistedException<>).Name, HandleEntityNotPersistedException },
            };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.TryGetValue(exceptionType.Name, out Func<HttpContext, Exception, Task> handler))
        {
            await handler.Invoke(httpContext, exception);

            return true;
        }

        return false;
    }

    private async Task HandleEntityNotFoundException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        });
    }

    private async Task HandleEntityUnprocessableException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "The provided request payload was not processable.",
            Detail = exception.Message
        });
    }

    private async Task HandleEntityNotPersistedException(HttpContext httpContext, Exception exception)
    {

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.5",
            Title = "Request processed successfully, but the entity was not persisted.",
            Detail = exception.Message
        });
    }
}
