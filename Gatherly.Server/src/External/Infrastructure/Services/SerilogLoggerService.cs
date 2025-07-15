using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SerilogLoggerService<TCategoryName>(ILogger<TCategoryName> logger) : IApplicationLoggerService<TCategoryName>
{
    private readonly ILogger<TCategoryName> _logger = logger;

    public void LogInformation(string message) =>
        _logger.LogInformation("Information: {Message}", message);

    public void LogWarning(string message) =>
        _logger.LogWarning("Warning: {Message}", message);

    public void LogError(Exception exception, string message) =>
        _logger.LogError(exception, "Error: {Message}", message);

    public void LogCritical(Exception exception, string message) =>
        _logger.LogCritical(exception, "Critical: {Message}", message);
}