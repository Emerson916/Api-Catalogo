namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.logLevel;
    }

    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        return null;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
    {
        DateTime dateNow = DateTime.Now;
        string mensagem = $"{dateNow} : {logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminhoArquivoLog = @"C:\Users\estagiario\Projetos\pessoal\APICatalogo\log\ApiCatalogo_Log.txt";
        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
