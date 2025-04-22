using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface IJwtGeradorToken{
    string GearToken(Usuario usuario);

}
