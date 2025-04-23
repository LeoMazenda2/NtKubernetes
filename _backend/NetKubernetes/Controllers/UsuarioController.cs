using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuarioDTO;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _repository = usuarioRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] UsuarioLoginRequestDto request)
    {
      return await _repository.Login(request);
    }


    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Registar([FromBody] UsuaioRegistoRequesteDto request)
    {
        return await _repository.RegistarUsuario(request);
    }


    [HttpGet("registar")]
    public async Task<ActionResult<UsuarioResponseDto>> DevolverUsuario()
    {
        return await _repository.GetUsuario();
    }

}
