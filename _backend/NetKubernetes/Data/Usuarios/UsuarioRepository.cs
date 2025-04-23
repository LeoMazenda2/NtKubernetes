using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuarioDTO;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;
using System.Net;

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

            if (usuario == null)
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { mensagem = "O Token desse usuário, não existe no nosso banco de dados" });

            return TranformerUserToUserDto(usuario!);
        }

        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email!);

            if (usuario == null)
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { mensagem = "O o email utilizado, não existe no nosso banco de dados" });

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);

            if (resultado.Succeeded)
                return TranformerUserToUserDto(usuario!);

            throw new MiddlewareException(
                HttpStatusCode.Unauthorized,
                new { mensagem = "Suas Credenciais estão Incorrectas" });
        }

        public async Task<UsuarioResponseDto> RegistarUsuario(UsuaioRegistoRequesteDto request)
        {
            var existeEmail = await _context.Users.Where(u => u.Email == request.Email).AnyAsync();

            if (existeEmail)
                throw new MiddlewareException(
                  HttpStatusCode.BadRequest,
                  new { mensagem = $"Este email ('{request.Email}') ja está sendo usado no sistema" });

            var existeUsername = await _context.Users.Where(u => u.UserName == request.UserName).AnyAsync();

            if (existeUsername)
                throw new MiddlewareException(
                  HttpStatusCode.BadRequest,
                  new { mensagem = $"Ja existe um registo usando esse nome de usuário: '{request.UserName}'" });

            var usuario = new Usuario {
                Nome = request.Nome,
                Apelido = request.Apelido,
                Telefone = request.Telefone,
                Email = request.Email,
                UserName = request.UserName
            };
            
            var resultado =  await _userManager.CreateAsync(usuario, request.Password!);

            if (resultado.Succeeded) {
                return TranformerUserToUserDto(usuario);
            }
            throw new Exception("Não foi possivel registar o usuário");

        }
    }
}
