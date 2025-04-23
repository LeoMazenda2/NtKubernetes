using NetKubernetes.Models;

namespace NetKubernetes.Data.Imoveis;

public interface IImovelRepository {
    Task<bool> SaveChange();
    Task<IEnumerable<Imovel>> GetAll();
    Task<Imovel> GetById(int id);
    Task Create(Imovel imovel);
    Task Delete(int id);
}
