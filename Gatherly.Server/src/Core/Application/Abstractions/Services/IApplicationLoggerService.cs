namespace Application.Abstractions.Services;

public interface IApplicationLoggerService<out TCategoryName>
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(Exception exception, string message);
    void LogCritical(Exception exception, string message);
}