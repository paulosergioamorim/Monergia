namespace Monergia.Requests;

public class CreateCondominioRequest
{
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public required string Name { get; set; }
    public required string? Observations { get; set; }
    
    // Telefones do síndico (array de string, compatível com PostgreSQL)
    public string[] TelefonesSindico { get; set; } = [];
    
    public required string TelefoneAdmin { get; set; }
    
    public required Guid SubrotaId { get; set; }
    
    public required string CEP { get; set; }
    public required string Logradouro { get; set; }
    public required string Bairro { get; set; }
    public required string Municipio { get; set; }
    public required string Estado { get; set; }
    public required int Numero { get; set; }

    // Listas informadas no momento do cadastro/atualização
    public IEnumerable<CreateAcessoRequest> Acessos { get; set; } = [];
    public IEnumerable<CreateElevadorRequest> Elevadores { get; set; } = [];
}