using LanguageExt;
using LanguageExt.Common;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public interface ICondominioService
{
    Task<Result<CondominioResponse>> CreateCondominioAsync(CreateCondominioRequest request);
    Task<Result<CondominioResponse>> UpdateCondominioAsync(Guid condominioId, CreateCondominioRequest request);
    Task<Result<Unit>> DeleteCondominioAsync(Guid condominioId);

    Task<CondominioResponse?> GetCondominioByIdAsync(Guid id);
    Task<IEnumerable<CondominioResponse>> GetAllCondominiosAsync();
}