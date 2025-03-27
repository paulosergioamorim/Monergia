using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monergia.Configs;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Services;

namespace Monergia.Controllers;

[ApiController, Route("filiais"), Authorize]
public class FiliaisController : ControllerBase
{
    private readonly IFilialService _filialService;

    public FiliaisController(IFilialService filialService) => _filialService = filialService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Filial>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAllAsync() => Results.Ok(await _filialService.GetAllFiliaisAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Filial), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IResult> GetByIdAsync(Guid id)
    {
        var filial = await _filialService.GetFilialByIdAsync(id);

        return filial != null ? Results.Ok(filial) : Results.NotFound();
    }

    [HttpPost("new")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateAsync(CreateFilialRequest filial)
    {
        var result = await _filialService.CreateFilialAsync(filial);

        return result.Match(_ => Results.Created(), ex => Results.BadRequest(ex.Message));
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateAsync(Guid id, CreateFilialRequest filial)
    {
        var result = await _filialService.UpdateFilialByIdAsync(id, filial);

        return result.Match(Results.Ok, ex => Results.BadRequest(ex.Message));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdministradorPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteAsync(Guid id)
    {
        var result = await _filialService.DeleteFilialByIdAsync(id);

        return result.Match(Results.Ok, ex => Results.BadRequest(ex.Message));
    }
}