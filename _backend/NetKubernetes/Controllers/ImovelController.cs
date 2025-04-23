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
    public async Task<ActionResult<IEnumerable<ImovelResponseDto>>> GetImoveis()
    {
        var imoveis = await _repository.GetAll();
        return Ok(_mapper.Map<IEnumerable<ImovelResponseDto>>(imoveis));
    }

    [HttpGet("{id}", Name = "GetImovelById")]
    public async Task<ActionResult<ImovelResponseDto>> GetImovelById(int id)
    {
        var imovel = await _repository.GetById(id);
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
    public async Task<ActionResult<ImovelResponseDto>> CreateImovel([FromBody] ImovelRequestDto imovel)
    {
        if (imovel is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensagem = "Ocorreu um erro ao criar o imóvel" }
            );
        }
        var imovelModel = _mapper.Map<Models.Imovel>(imovel);
        await _repository.Create(imovelModel);
        await _repository.SaveChange();

        var imovelResponse = _mapper.Map<ImovelResponseDto>(imovelModel);

        return CreatedAtRoute(nameof(GetImovelById), new { id = imovelResponse.Id }, imovelResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteImovel(int id)
    {
        var imovelModel = await _repository.GetById(id);
        if (imovelModel is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.NotFound,
                new { mensagem = $"Não foi encontrado o id: {id}" }
            );
        }
        await _repository.Delete(id);
        await _repository.SaveChange();
        return Ok();
    }
}
