using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SerilogLoggerService<TCategoryName>(ILogger<TCategoryName> logger) : IApplicationLoggerService<TCategoryName>
{
    private readonly ILogger<TCategoryName> _logger = logger;

    public void LogInformation(string message) =>
        _logger.LogInformation("Information: {Message}", message);

    public void LogInformation(string message, params object[] args) =>
        _logger.LogInformation("Information: {Message}", [message, .. args]);

    public void LogWarning(string message) =>
        _logger.LogWarning("Warning: {Message}", message);

    public void LogWarning(string message, params object[] args) =>
        _logger.LogWarning("Warning: {Message}", [message, .. args]);

    public void LogError(string message) =>
        _logger.LogError("Error: {Message}", message);

    public void LogError(string message, params object[] args) =>
        _logger.LogError("Error: {Message}", [message, .. args]);

    public void LogError(Exception exception, string message) =>
        _logger.LogError(exception, "Error: {Message}", message);

    public void LogError(Exception exception, string message, params object[] args) =>
        _logger.LogError(exception, "Error: {Message}", [message, .. args]);

    public void LogCritical(string message) =>
        _logger.LogCritical("Critical: {Message}", message);

    public void LogCritical(string message, params object[] args) =>
        _logger.LogCritical("Critical: {Message}", [message, .. args]);

    public void LogCritical(Exception exception, string message) =>
        _logger.LogCritical(exception, "Critical: {Message}", message);

    public void LogCritical(Exception exception, string message, params object[] args) =>
        _logger.LogCritical(exception, "Critical: {Message}", [message, .. args]);
}