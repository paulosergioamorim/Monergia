using System.ComponentModel.DataAnnotations;

namespace Monergia.Requests;

public class CreateAcessoRequest
{
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}