using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class Condominio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(18)] public string? CNPJ { get; set; }
    
    [MaxLength(14)] public string? CPF { get; set; }
    
    [MaxLength(100)] public required string Name { get; set; }
    
    [MaxLength(255)] public required string? Observations { get; set; }
    
    [StringLength(255)] public string[] TelefonesSincico { get; set; } = [];
    
    [MaxLength(255)] public required string TelefoneAdmin { get; set; }
    
    public int QuantidadeElevadores => Elevadores.Count;
    
    public Guid SubrotaId { get; set; }
    
    public Subrota Subrota { get; set; } = null!;
    
    public StatusCliente Status { get; set; }
    
    public Guid EnderecoId { get; set; }
    
    [MaxLength(9)] public required string CEP { get; set; }
    
    [MaxLength(95)] public required string Logradouro { get; set; }
    
    [MaxLength(95)] public required string Bairro { get; set; }
    
    [MaxLength(95)] public required string Municipio { get; set; }
    
    [MaxLength(2)] public required string Estado { get; set; }
    
    public required int Numero { get; set; }
    
    public List<Elevador> Elevadores { get; set; } = [];
    
    public List<VisitaTecnica> VisitasTecnicas { get; set; } = [];
    
    public List<Acesso> Acessos { get; set; } = [];
}