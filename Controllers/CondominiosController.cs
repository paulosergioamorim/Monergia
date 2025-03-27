using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monergia.Configs;
using Monergia.Requests;
using Monergia.Response;
using Monergia.Services;

namespace Monergia.Controllers;

[ApiController]
[Route("condominios")]
[Authorize]
public class CondominiosController : ControllerBase
{
    private readonly ICondominioService _condominioService;

    public CondominiosController(ICondominioService condominioService)
    {
        _condominioService = condominioService;
    }

    // GET /condominios
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CondominioResponse>), 200)]
    public async Task<IResult> GetAllAsync()
    {
        var lista = await _condominioService.GetAllCondominiosAsync();
        return Results.Ok(lista);
    }

    // GET /condominios/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CondominioResponse), 200)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<IResult> GetByIdAsync(Guid id)
    {
        var cond = await _condominioService.GetCondominioByIdAsync(id);
        return cond != null ? Results.Ok(cond) : Results.NotFound();
    }

    // POST /condominios/new
    [HttpPost("new")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(typeof(CondominioResponse), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> CreateAsync([FromBody] CreateCondominioRequest request)
    {
        var result = await _condominioService.CreateCondominioAsync(request);
        return result.Match(
            sucesso =>
            {
                // Retorna 201 + dados do condomínio criado
                return Results.Created($"/condominios/{sucesso.Id}", sucesso);
            },
            erro => Results.BadRequest(erro.Message)
        );
    }

    // PATCH /condominios/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(typeof(CondominioResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> UpdateAsync(Guid id, [FromBody] CreateCondominioRequest request)
    {
        var result = await _condominioService.UpdateCondominioAsync(id, request);
        return result.Match(
            sucesso => Results.Ok(sucesso),
            erro => Results.BadRequest(erro.Message)
        );
    }

    // DELETE /condominios/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> DeleteAsync(Guid id)
    {
        var result = await _condominioService.DeleteCondominioAsync(id);
        return result.Match(
            _ => Results.Ok(),
            erro => Results.BadRequest(erro.Message)
        );
    }
}
