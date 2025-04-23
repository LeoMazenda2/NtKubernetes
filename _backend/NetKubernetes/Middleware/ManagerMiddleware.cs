using Newtonsoft.Json;
using System.Net;

namespace NetKubernetes.Middleware;

public class ManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManagerMiddleware> _logger;

    public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManagerExceptionAsync(context, ex, _logger);
        }
    }

    private async Task ManagerExceptionAsync(HttpContext context, Exception ex, ILogger<ManagerMiddleware> logger)
    {
        object? erros = null;

        switch (ex)
        {
            case MiddlewareException me:
                logger.LogError(me, "Middleware Error");
                erros = me.Erros;
                context.Response.StatusCode = (int)me.Codigo;
                break;

            case Exception ex:
                logger.LogError(ex, "eRRO DE Servidor");
                erros = string.IsNullOrWhiteSpace(ex.Message) ? "Erro" : ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";
        var resultados = string.Empty;

        if (erros != null)
            resultados = JsonConvert.SerializeObject(new { erros });

        await context.Response.WriteAsync(resultados);

    }
}