using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Monergia.Requests;

public class UpdateUserRequest
{
    public DateOnly? BirthDate { get; set; }

    [MaxLength(100)] public string? Name { get; set; }

    [MaxLength(100), Comment("Login de acesso na plataforma")]
    public string? Username { get; set; }

    [MinLength(6, ErrorMessage = "A senha é muito curta!"), PasswordPropertyText]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem!"), PasswordPropertyText]
    public string? ConfirmPassword { get; set; }

    public Guid? FilialId { get; set; }

    public string? Role { get; set; }
}