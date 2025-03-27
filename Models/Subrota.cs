using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class Subrota
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(10)]
    public required string Codigo { get; set; }
    
    public Guid RotaId { get; set; }
    
    public Rota Rota { get; set; } = null!;
}