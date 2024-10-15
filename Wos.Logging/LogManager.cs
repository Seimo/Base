using System;

namespace Wos.Logging
{
    /// <summary>
    /// Used to manage logging.
    /// </summary>
    public static class LogManager
    {

        private static readonly ILog NullLogInstance = new NullLog();

        /// <summary>
        /// Creates an <see cref="T:Wos.Logging.ILog" /> for the provided type.
        /// </summary>
        public static Func<Type, ILog> GetLog = type => NullLogInstance;

        private class NullLog : ILog
        {
            public void Info(string format, params object[] args)
            {
                Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {format}");
            }

            public void Warn(string format, params object[] args)
            {
                Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {format}");
            }

            public void Error(Exception exception)
            {
                Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {exception.GetBaseException().Message}");
            }
            

        }

    }

}
