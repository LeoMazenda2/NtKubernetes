using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface IJwtGeradorToken{
    string GerarToken(Usuario usuario);

}
