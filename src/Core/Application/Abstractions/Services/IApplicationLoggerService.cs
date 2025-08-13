namespace Application.Abstractions.Services;

public interface IApplicationLoggerService<out TCategoryName>
{
    void LogInformation(string message);
    void LogInformation(string message, params object[] args);

    void LogWarning(string message);
    void LogWarning(string message, params object[] args);

    void LogError(string message);
    void LogError(string message, params object[] args);
    void LogError(Exception exception, string message);
    void LogError(Exception exception, string message, params object[] args);

    void LogCritical(string message);
    void LogCritical(string message, params object[] args);
    void LogCritical(Exception exception, string message);
    void LogCritical(Exception exception, string message, params object[] args);
}