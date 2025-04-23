using System.Net;

namespace NetKubernetes.Middleware;

public class MiddlewareException : Exception
{
    public HttpStatusCode Codigo { get; set; }
    public object? Erros { get; set; }

    public MiddlewareException(HttpStatusCode codigo, object? erros = null)
    {
        Codigo = codigo;
        Erros = erros;
    }


}
