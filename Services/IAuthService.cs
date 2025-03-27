using LanguageExt;
using LanguageExt.Common;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public interface IAuthService
{
    public Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest);

    public Task<Result<AuthResponse>> RegisterAsync(CreateUserRequest userRequest);

    public Task<Result<Unit>> UpdateUserAsync(Guid id, UpdateUserRequest userRequest);

    public Task<Result<UserResponse>> GetUserByIdAsync(Guid id);
}