namespace APICatalogo.Logging;

//Classe de configuração do Logger personalizado

public class CustomLoggerProviderConfiguration
{
    //LogLevel - Define o nível mínimo de log a ser registrado, com o padrão LogLevel.Warning
    public LogLevel logLevel { get; set; } = LogLevel.Warning;
    //EventId - Define o ID do evento de log com o padrão sendo zero
    public int EventId { get; set; } = 0;
}