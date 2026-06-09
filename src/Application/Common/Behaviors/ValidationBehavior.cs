using FluentValidation;
using MediatR;

/// <summary>
/// Use FluentValidator to validate the request as a part of the pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    // Get all validators that is this Request type
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(
                    new ValidationContext<TRequest>(request),
                    cancellationToken
                ))
            );

            var failures = validationResults.Where(
                r => r.Errors.Count != 0    // Result is nothing skip.
            ).SelectMany(r => r.Errors) // Flat out all errors.
            .ToList(); // All to list

            if (failures.Count != 0) // Is there is any wrong throw
                throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}