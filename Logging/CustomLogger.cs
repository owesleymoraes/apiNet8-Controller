
namespace ApiCatalogo.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
        {
            this.loggerName = loggerName;
            this.loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            fileMessage(message);
        }

        private void fileMessage(string message)
        {
            string pathFileLog = @"d:\app_web\ApiNet8\wesley_log.txt";
            using (StreamWriter streamWriter = new StreamWriter(pathFileLog, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();

                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }
    }
}