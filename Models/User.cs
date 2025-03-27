using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateOnly BirthDate { get; set; }
    
    [MaxLength(95)] public required string Name { get; set; }
    
    [MaxLength(95)] public required string Username { get; set; }
    
    [MaxLength(255)] public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(95)] public required string Role { get; set; }
    
    public Guid FilialId { get; set; }
    
    public Filial Filial { get; set; } = null!;
}