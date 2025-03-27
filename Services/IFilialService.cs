using LanguageExt;
using LanguageExt.Common;
using Monergia.Models;
using Monergia.Requests;

namespace Monergia.Services;

public interface IFilialService
{
    public Task<IEnumerable<Filial>> GetAllFiliaisAsync();

    public Task<Filial?> GetFilialByIdAsync(Guid id);

    public Task<Filial?> GetFilialByNomeAsync(string nome);

    public Task<Result<Unit>> CreateFilialAsync(CreateFilialRequest filialRequest);

    public Task<Result<Unit>> UpdateFilialByIdAsync(Guid id, CreateFilialRequest filial);

    public Task<Result<Unit>> DeleteFilialByIdAsync(Guid id);
}