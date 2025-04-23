using AutoMapper;
using NetKubernetes.Dtos.ImovelDTO;
using NetKubernetes.Models;

namespace NetKubernetes.Profiles;

public class ImovelProfile : Profile
{
    public ImovelProfile()
    {
        CreateMap<Imovel, ImovelResponseDto>();          
        CreateMap<ImovelRequestDto, Imovel>();          
    }
}