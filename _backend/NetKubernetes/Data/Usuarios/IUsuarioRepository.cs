using NetKubernetes.Dtos.UsuarioDTO;

namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepository
{
    Task<UsuarioResponseDto> GetUsuario();
    Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request);
    Task<UsuarioResponseDto> RegistarUsuario(UsuaioRegistoRequesteDto request);
    
}
