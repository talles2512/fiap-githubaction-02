using System.Reflection;

namespace FiapStore.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _loggerConfig;

        public CustomLogger(string nome, CustomLoggerProviderConfiguration loggerConfig)
        {
            _loggerName = nome;
            _loggerConfig = loggerConfig;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var mensagem = string.Format($"{logLevel}: {eventId}" +
                $" - {formatter(state, exception)}");

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            var fileName = @$"{Directory.GetCurrentDirectory()}\{DateTime.Now:yyyy_MM_dd}.txt";

            using var streamWriter = new StreamWriter(fileName, true);
            streamWriter.WriteLine(mensagem);
            streamWriter.Close();
        }
    }
}