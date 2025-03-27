using System.ComponentModel.DataAnnotations;

namespace Monergia.Requests;

public class CreateRotaRequest
{
    [MaxLength(50)]
    public required string Regiao { get; set; }

    [MaxLength(10)]
    public required string Codigo { get; set; }

    [MaxLength(100)]
    public required string Nome { get; set; }
}