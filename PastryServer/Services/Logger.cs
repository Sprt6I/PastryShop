using System.Runtime.CompilerServices;

namespace PastryServer.Services
{
    public static class Logger
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        public static void Log(string message, LogLevel level, [CallerMemberName] string caller = "", [CallerFilePath] string? file = "", [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"[{DateTime.Now}] [{file}:[{caller}]:{line}]: [{level}] {message}");
        }
    }
}
