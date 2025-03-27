using System.ComponentModel.DataAnnotations;

namespace Monergia.Requests;

public class CreateSubrotaRequest
{
    [MaxLength(10)]
    public required string Codigo { get; set; }
    
    [Required]
    public Guid RotaId { get; set; }
}