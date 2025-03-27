using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class Rota
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(50)]
    public required string Regiao { get; set; }
    
    [MaxLength(10)]
    public required string Codigo { get; set; }
    
    [MaxLength(100)]
    public required string Nome { get; set; }

    public List<Subrota> Subrotas { get; set; } = [];
}