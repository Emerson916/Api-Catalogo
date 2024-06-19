using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context) {
        //Execute antes da Action
        _logger.LogInformation($"##-##########################-##");
        _logger.LogInformation($"##-Executando antes da Action-##");
        _logger.LogInformation($"##-##########################-##");
    }

    public void OnActionExecuted(ActionExecutedContext context) {
        //Executa depois da Action 
        _logger.LogInformation($"##-##########################-##");
        _logger.LogInformation($"##- Executeou - Status Code -##  {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation($"##-##########################-##");
    }
}