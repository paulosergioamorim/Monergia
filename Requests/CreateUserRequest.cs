using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Monergia.Requests;

public class CreateUserRequest
{
    public required DateOnly BirthDate { get; set; }
    
    [MaxLength(100)] public required string Name { get; set; }
    
    [MaxLength(100), Comment("Login de acesso na plataforma")]
    public required string Username { get; set; }
    
    [MinLength(6, ErrorMessage = "A senha é muito curta!"), PasswordPropertyText]
    public required string Password { get; set; }
    
    [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem!"), PasswordPropertyText]
    public required string ConfirmPassword { get; set; }
    
    public required Guid FilialId { get; set; }
    
    public required string Role { get; set; }
}