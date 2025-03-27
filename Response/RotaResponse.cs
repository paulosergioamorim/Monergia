using Monergia.Models;

namespace Monergia.Response;

public class RotaResponse
{
    private readonly Rota _rota;

    public Guid Id => _rota.Id;
    public string Codigo => _rota.Codigo;
    public string Regiao => _rota.Regiao;
    public string Nome => _rota.Nome;

    public RotaResponse(Rota rota) => _rota = rota;
}