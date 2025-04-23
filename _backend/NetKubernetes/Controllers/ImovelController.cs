using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Imoveis;
using NetKubernetes.Dtos.ImovelDTO;
using NetKubernetes.Middleware;
using System.Net;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImovelController : ControllerBase
{
    private readonly IImovelRepository _repository;
    private IMapper _mapper;
    public ImovelController(IImovelRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ImovelResponseDto>> GetImoveis()
    {
        var imoveis = _repository.GetAll();
        return Ok(_mapper.Map<IEnumerable<ImovelResponseDto>>(imoveis));
    }

    [HttpGet("{id}", Name = "GetImovelById")]
    public ActionResult<ImovelResponseDto> GetImovelById(int id)
    {
        var imovel = _repository.GetById(id);
        if (imovel is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.NotFound,
                new { mensagem = $"Não foi encontrado o id: {id}" }
             );
        }
        return Ok(_mapper.Map<ImovelResponseDto>(imovel));
    }

    [HttpPost]
    public ActionResult<ImovelResponseDto> CreateImovel([FromBody] ImovelRequestDto imovel)
    {
        if (imovel is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensagem = "Ocorreu um erro ao criar o imóvel" }
            );
        }
        var imovelModel = _mapper.Map<Models.Imovel>(imovel);
        _repository.Create(imovelModel);
        _repository.SaveChange();

        var imovelResponse = _mapper.Map<ImovelResponseDto>(imovelModel);

        return CreatedAtRoute(nameof(GetImovelById), new { id = imovelResponse.Id }, imovelResponse);
    }   

    [HttpDelete("{id}")]
    public ActionResult DeleteImovel(int id)
    {
        var imovelModel = _repository.GetById(id);
        if (imovelModel is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.NotFound,
                new { mensagem = $"Não foi encontrado o id: {id}" }
            );
        }
        _repository.Delete(id);
        _repository.SaveChange();
        return NoContent();
    }
}