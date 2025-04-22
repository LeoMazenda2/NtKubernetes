using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDTO;
using NetKubernetes.Models;
using NetKubernetes.Token;
using System.Net.Http.Headers;

namespace NetKubernetes.Data.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IJwtGeradorToken _jwtGeradorToken;
        private readonly AppDbContext _context;
        private readonly IUsuarioSessao _usuarioSessao;

        public UsuarioRepository(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IJwtGeradorToken jwtGeradorToken,
            AppDbContext context,
            IUsuarioSessao usuarioSessao)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGeradorToken = jwtGeradorToken;
            _context = context;
            _usuarioSessao = usuarioSessao;
        }

        private UsuarioResponseDto TranformerUserToUserDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Token = _jwtGeradorToken.GerarToken(usuario)
            };
        }

        public async Task<UsuarioResponseDto> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSessao.ObterUsuarioSessao());
            return TranformerUserToUserDto(usuario!);
        }

        public async Task<UsuarioResponseDto> Login(UsuaioLoginRequestDto request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email!);
            await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);
            return TranformerUserToUserDto(usuario!);
        }

        public async Task<UsuarioResponseDto> RegistarUsuario(UsuaioRegistoRequesteDto request)
        {
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Apelido = request.Apelido,
                Telefone = request.Telefone,
                Email = request.Email,
                UserName = request.UserName
            };
            await _userManager.CreateAsync(usuario, request.Password!);

            return TranformerUserToUserDto(usuario);

        }
    }
}
