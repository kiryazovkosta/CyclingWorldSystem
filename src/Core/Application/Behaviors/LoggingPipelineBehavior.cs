namespace Application.Behaviors;

using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        this._logger.LogInformation(
            "Starting request {@RequestName}, {@Request} {@DateTimeUtc}", 
            typeof(TRequest).Name,
            request,
            DateTime.UtcNow);
        
        var result = await next();

        if (result.IsFailure)
        {
            this._logger.LogError(
                "Request failure {@RequestName}, {@Error} {@DateTimeUtc}",
                typeof(TRequest).Name,
                result.Error,
			    DateTime.UtcNow);
		}
        
        this._logger.LogInformation(
            "Completed request {@RequestName}, {@DateTimeUtc}", 
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}