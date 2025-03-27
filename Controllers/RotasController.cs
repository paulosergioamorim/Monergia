using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monergia.Configs;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Response;
using Monergia.Services;

namespace Monergia.Controllers;

[ApiController]
[Route("rotas")]
[Authorize]
public class RotasController : ControllerBase
{
    private readonly IRotaService _rotaService;

    public RotasController(IRotaService rotaService)
    {
        _rotaService = rotaService;
    }

    // =====================================
    //          MÉTODOS DE ROTA
    // =====================================
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Rota>), 200)]
    public async Task<IResult> GetAllAsync()
        => Results.Ok(await _rotaService.GetAllRotasAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Rota), 200)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<IResult> GetByIdAsync(Guid id)
    {
        var rota = await _rotaService.GetRotaByIdAsync(id);
        return rota != null ? Results.Ok(rota) : Results.NotFound();
    }

    [HttpPost("new")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> CreateAsync([FromBody] CreateRotaRequest request)
    {
        var result = await _rotaService.CreateRotaAsync(request);
        return result.Match(
            _ => Results.Created($"/rotas/{request.Codigo}", null),
            ex => Results.BadRequest(ex.Message)
        );
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> UpdateAsync(Guid id, [FromBody] CreateRotaRequest request)
    {
        var result = await _rotaService.UpdateRotaAsync(id, request);
        return result.Match(
            _ => Results.Ok(),
            ex => Results.BadRequest(ex.Message)
        );
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> DeleteAsync(Guid id)
    {
        var result = await _rotaService.DeleteRotaAsync(id);
        return result.Match(
            _ => Results.Ok(),
            ex => Results.BadRequest(ex.Message)
        );
    }

    // =====================================
    //         MÉTODOS DE SUBROTA
    // =====================================
    [HttpGet("{rotaId:guid}/subrotas")]
    [ProducesResponseType(typeof(IEnumerable<SubrotaResponse>), 200)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<IResult> GetSubrotasByRotaIdAsync(Guid rotaId)
    {
        var result = await _rotaService.GetSubrotasByRotaIdAsync(rotaId);
        return result.Match(
            Results.Ok,
            ex => Results.BadRequest(ex.Message)
        );
    }

    [HttpPost("{rotaId:guid}/subrotas/new")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> CreateSubrotaAsync(Guid rotaId, [FromBody] CreateSubrotaRequest request)
    {
        var result = await _rotaService.AddSubrotaAsync(rotaId, request);
        return result.Match(
            _ => Results.Created($"/rotas/{rotaId}/subrotas/{request.Codigo}", null),
            ex => Results.BadRequest(ex.Message)
        );
    }

    [HttpPatch("{rotaId:guid}/subrotas/{subrotaId:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> UpdateSubrotaAsync(Guid rotaId, Guid subrotaId, [FromBody] CreateSubrotaRequest request)
    {
        var result = await _rotaService.UpdateSubrotaAsync(rotaId, subrotaId, request);
        return result.Match(
            _ => Results.Ok(),
            ex => Results.BadRequest(ex.Message)
        );
    }

    [HttpDelete("{rotaId:guid}/subrotas/{subrotaId:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> DeleteSubrotaAsync(Guid rotaId, Guid subrotaId)
    {
        var result = await _rotaService.DeleteSubrotaAsync(rotaId, subrotaId);
        return result.Match(
            _ => Results.Ok(),
            ex => Results.BadRequest(ex.Message)
        );
    }
}
