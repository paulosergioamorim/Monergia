namespace Monergia.Response;

public class AuthResponse
{
    public AuthResponse(string token, UserResponse user)
    {
        Token = token;
        User = user;
    }

    public string Token { get; private set; }
    
    public UserResponse User { get; private set; }
}