using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;
using System.Net;
using System.Threading.Tasks;

namespace NetKubernetes.Data.Imoveis
{
    public class ImovelRepository : IImovelRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioSessao _usuarioSessao;
        private readonly UserManager<Usuario> _userManager;
        public ImovelRepository(AppDbContext context,
            IUsuarioSessao usuarioSessao,
            UserManager<Usuario> userManager)
        {

            _context = context;
            _usuarioSessao = usuarioSessao;
            _userManager = userManager;
        }

        public async Task Create(Imovel imovel)
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSessao.ObterUsuarioSessao());
            if (usuario == null)
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { Mensagem = "Usuário não é válido para fazer essa inserção" }
                    );

            if (imovel is null)
                throw new MiddlewareException(
                    HttpStatusCode.BadRequest,
                    new { Mensagem = "Imóvel não pode ser nulo" }
                    );

            imovel.DataCriacao = DateTime.Now;
            imovel.UsuarioId = Guid.Parse(usuario!.Id);

            await _context.Imoveis!.AddAsync(imovel);
        }

        public async Task Delete(int id)
        {
            var imovel = await _context.Imoveis!.FirstOrDefaultAsync(i => i.Id == id);
            //if (imovel == null) throw new Exception("Imovel não encontrado");
             _context.Imoveis!.Remove(imovel!);
        }

        public async Task<IEnumerable<Imovel>> GetAll()
        {
            return await _context.Imoveis!.ToListAsync();
        }

        public async Task<Imovel> GetById(int id)
        {
            return await _context.Imoveis!.FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public async Task<bool> SaveChange()
        {
            return (await _context.SaveChangesAsync() > 0);

        }
    }
}
