using MediatR;
using Microsoft.Extensions.Logging;

namespace RentalSystem.Application.Common.Behaviors;

/// <summary>
/// Warps all pipeline that throws to fallback to here and log it.
/// This is the first in the pipeline behavior.
/// TODO: might can post it to monitoring interface
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private ILogger _logger;

    public UnhandledExceptionBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try // warps all the next pipeline when error occurs this handle it
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}