using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Monergia.Models;

public class Acesso
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(14)] public string? CPF { get; set; }
    
    [MaxLength(18)] public string? CNPJ { get; set; }
    
    [MaxLength(100)] public required string Name { get; set; }

    [MaxLength(255), JsonIgnore] public string PasswordHash { get; set; } = string.Empty;
    
    public Guid CondominioId { get; set; }
    
    [JsonIgnore]
    public Condominio Condominio { get; set; } = null!;
}