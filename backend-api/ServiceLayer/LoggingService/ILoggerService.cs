using System;

namespace HrManagementSystem.ServiceLayer.LoggingService
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception ex, string message);
        void LogCritical(string message);
        void LogCritical(Exception ex, string message);
    }
}