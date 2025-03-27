using LanguageExt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monergia.Requests;
using Monergia.Response;
using Monergia.Services;

namespace Monergia.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IResult> LoginAsync(LoginRequest loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);

        return result.Match(Results.Ok, ex => Results.BadRequest(ex.Message));
    }

    [HttpPost("register")]
    public async Task<IResult> RegisterAsync(CreateUserRequest userRequest)
    {
        var result = await _authService.RegisterAsync(userRequest);

        return result.Match(_ => Results.Created(), ex => Results.BadRequest(ex.Message));
    }

    [HttpPatch("{id:guid}"), Authorize]
    public async Task<IResult> UpdateAsync(Guid id, UpdateUserRequest userRequest)
    {
        await _authService.UpdateUserAsync(id, userRequest);

        return Results.Ok();
    }
}