using LanguageExt;
using LanguageExt.Common;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public interface IRotaService
{
    Task<IEnumerable<Rota>> GetAllRotasAsync();
    Task<Rota?> GetRotaByIdAsync(Guid id);
    Task<Rota?> GetRotaByCodigoAsync(string codigo);
    Task<Result<Unit>> CreateRotaAsync(CreateRotaRequest request);
    Task<Result<Unit>> UpdateRotaAsync(Guid id, CreateRotaRequest request);
    Task<Result<Unit>> DeleteRotaAsync(Guid id);

    // --- Métodos para Subrotas ---
    Task<Result<IEnumerable<SubrotaResponse>>> GetSubrotasByRotaIdAsync(Guid rotaId);
    Task<Result<Unit>> AddSubrotaAsync(Guid rotaId, CreateSubrotaRequest subrotaRequest);
    Task<Result<Unit>> UpdateSubrotaAsync(Guid rotaId, Guid subrotaId, CreateSubrotaRequest subrotaRequest);
    Task<Result<Unit>> DeleteSubrotaAsync(Guid rotaId, Guid subrotaId);
}