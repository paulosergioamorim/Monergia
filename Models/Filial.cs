using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class Filial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(100)] public required string Name { get; set; }
}