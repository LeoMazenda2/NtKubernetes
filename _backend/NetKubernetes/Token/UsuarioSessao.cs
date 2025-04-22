using System.Security.Claims;

namespace NetKubernetes.Token
{
    public class UsuarioSessao : IUsuarioSessao
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSessao(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObterUsuarioSessao()
        {
            var serName = _httpContextAccessor.HttpContext.User?.Claims?
                             .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;   
            
            return serName!;
        }
    }
}
