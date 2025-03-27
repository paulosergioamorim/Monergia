using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class Elevador
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(100)]
    public required string Nomenclatura { get; set; }

    public Guid CondominioId { get; set; }
}