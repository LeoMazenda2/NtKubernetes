using NetKubernetes.Models;

namespace NetKubernetes.Data.Imoveis;

public interface IImovelRepository {
    bool SaveChange();
    IEnumerable<Imovel> GetAll();
    Imovel GetById(int id);
    Task Create(Imovel imovel);
    void Delete(int id);
}
