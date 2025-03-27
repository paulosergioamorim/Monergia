using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Monergia.DbContexts;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public class AuthService : IAuthService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IFilialService _filialService;

    public AuthService(IDbContextFactory<AppDbContext> dbContextFactory, IPasswordHasher<User> passwordHasher,
        IJwtService jwtService, IFilialService filialService)
    {
        _dbContextFactory = dbContextFactory;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _filialService = filialService;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        try
        {
            var existingUsuario = await db.Usuarios.SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (existingUsuario == null)
                return new Result<AuthResponse>(new ArgumentException("Usuário não existe"));

            var result =
                _passwordHasher.VerifyHashedPassword(existingUsuario, existingUsuario.PasswordHash,
                    loginRequest.Password);

            if (result == PasswordVerificationResult.Failed)
                return new Result<AuthResponse>(new ArgumentException("Senha incorreta"));

            var token = _jwtService.GenerateToken(existingUsuario);

            return new AuthResponse(token, new UserResponse(existingUsuario));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<AuthResponse>(e);
        }
    }

    public async Task<Result<AuthResponse>> RegisterAsync(CreateUserRequest userRequest)
    {
        try
        {
            var existingFilial = await _filialService.GetFilialByIdAsync(userRequest.FilialId);

            if (existingFilial == null)
                return new Result<AuthResponse>(new ArgumentException("Filial não existe"));

            var user = new User
            {
                Name = userRequest.Name,
                Username = userRequest.Username,
                BirthDate = userRequest.BirthDate,
                FilialId = userRequest.FilialId,
                Role = userRequest.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);

            await using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Usuarios.AddAsync(user);
            await db.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            var response = new AuthResponse(token, new UserResponse(user));

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<AuthResponse>(e);
        }
    }

    public async Task<Result<Unit>> UpdateUserAsync(Guid id, UpdateUserRequest userRequest)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        var existingUser = await db.Usuarios.FindAsync(id);

        if (existingUser == null)
            return new Result<Unit>(new ArgumentException("Usuário não existe."));

        if (userRequest.FilialId != null)
        {
            var existingFilial = await _filialService.GetFilialByIdAsync((Guid)userRequest.FilialId);

            if (existingFilial == null)
                return new Result<Unit>(new ArgumentException("Filial não existe."));
        }

        db.Entry(existingUser).CurrentValues.SetValues(userRequest);

        if (userRequest.Password != null)
        {
            var passwordHash = _passwordHasher.HashPassword(existingUser, userRequest.Password);
            existingUser.PasswordHash = passwordHash;
        }
        
        await db.SaveChangesAsync();

        return new Result<Unit>();
    }

    public async Task<Result<UserResponse>> GetUserByIdAsync(Guid id)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();

            var existingUser = await db.Usuarios.FindAsync(id);

            return existingUser == null
                ? new Result<UserResponse>(new ArgumentException("Usuário não existe"))
                : new UserResponse(existingUser);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<UserResponse>(e);
        }
    }
}