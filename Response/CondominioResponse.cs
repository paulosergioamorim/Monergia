using Monergia.Models;

namespace Monergia.Response;

public class CondominioResponse
{
    private readonly Condominio _condominio;
    public Guid Id => _condominio.Id;
    public string? CNPJ => _condominio.CNPJ;
    public string? CPF => _condominio.CPF;
    public string Name => _condominio.Name;
    public string? Observations => _condominio.Observations;
    public string[] TelefonesSindico => _condominio.TelefonesSincico;
    public string TelefoneAdmin => _condominio.TelefoneAdmin;
    public string CEP => _condominio.CEP;
    public string Logradouro => _condominio.Logradouro;
    public string Bairro => _condominio.Bairro;
    public string Municipio => _condominio.Municipio;
    public string Estado => _condominio.Estado;
    public int Numero => _condominio.Numero;

    public List<Acesso> Acessos => _condominio.Acessos;
    public List<Elevador> Elevadores => _condominio.Elevadores;

    public SubrotaResponse Subrota => new SubrotaResponse(_condominio.Subrota);
    public RotaResponse Rota => new RotaResponse(_condominio.Subrota.Rota);

    public CondominioResponse(Condominio condominio) => _condominio = condominio;
}
