using Monergia.Models;

namespace Monergia.Response;

public class SubrotaResponse
{
    private readonly Subrota _subrota;

    public SubrotaResponse(Subrota subrota) => _subrota = subrota;

    public Guid Id => _subrota.Id;

    public string Codigo => _subrota.Codigo;
}