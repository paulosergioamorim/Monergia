using System.ComponentModel.DataAnnotations;

namespace Monergia.Requests;

public class CreateFilialRequest
{
    [MaxLength(100)]
    public required string Name { get; set; }
}