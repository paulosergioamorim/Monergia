using Monergia.Models;

namespace Monergia.Services;

public interface IJwtService
{
    public string GenerateToken(User user);
}