using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Monergia.DbContexts;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public class RotaService : IRotaService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public RotaService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // ======================
    //  Métodos de ROTA
    // ======================
    public async Task<IEnumerable<Rota>> GetAllRotasAsync()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Rotas
            .Include(r => r.Subrotas)
            .ToListAsync();
    }

    public async Task<Rota?> GetRotaByIdAsync(Guid id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Rotas
            .Include(r => r.Subrotas)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rota?> GetRotaByCodigoAsync(string codigo)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Rotas
            .Include(r => r.Subrotas)
            .FirstOrDefaultAsync(r => r.Codigo == codigo);
    }

    public async Task<Result<Unit>> CreateRotaAsync(CreateRotaRequest request)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            if (await db.Rotas.AnyAsync(r => r.Codigo == request.Codigo))
                return new Result<Unit>(new ArgumentException("Código já existe"));

            var rota = new Rota
            {
                Regiao = request.Regiao,
                Codigo = request.Codigo,
                Nome = request.Nome
            };

            await db.Rotas.AddAsync(rota);
            await db.SaveChangesAsync();

            return new Result<Unit>();
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public async Task<Result<Unit>> UpdateRotaAsync(Guid id, CreateRotaRequest request)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync();
            var rota = await db.Rotas.FindAsync(id);

            if (rota == null)
                return new Result<Unit>(new ArgumentException("Rota não encontrada"));

            if (await db.Rotas.AnyAsync(r => r.Id != id && r.Codigo == request.Codigo))
                return new Result<Unit>(new ArgumentException("Código já está em uso"));

            rota.Regiao = request.Regiao;
            rota.Codigo = request.Codigo;
            rota.Nome = request.Nome;

            await db.SaveChangesAsync();
            return new Result<Unit>();
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public async Task<Result<Unit>> DeleteRotaAsync(Guid id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var rota = await db.Rotas
            .Include(r => r.Subrotas)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rota == null)
            return new Result<Unit>(new ArgumentException("Rota não encontrada"));

        if (rota.Subrotas.Any())
            return new Result<Unit>(new ArgumentException("Exclua as subrotas primeiro"));

        db.Rotas.Remove(rota);
        await db.SaveChangesAsync();

        return new Result<Unit>();
    }

    // ======================
    //  Métodos de SUBROTA
    // ======================

    public async Task<Result<IEnumerable<SubrotaResponse>>> GetSubrotasByRotaIdAsync(Guid rotaId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var rota = await db.Rotas
            .Include(r => r.Subrotas)
            .FirstOrDefaultAsync(r => r.Id == rotaId);

        if (rota == null)
            return new Result<IEnumerable<SubrotaResponse>>(new ArgumentException("Rota não encontrada"));

        // Converte as Subrotas para SubrotaResponse
        var subrotasResponse = rota.Subrotas
            .Select(s => new SubrotaResponse(s))
            .ToList();

        return subrotasResponse;
    }

    public async Task<Result<Unit>> AddSubrotaAsync(Guid rotaId, CreateSubrotaRequest subrotaRequest)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync();
            var rota = await db.Rotas
                .Include(r => r.Subrotas)
                .FirstOrDefaultAsync(r => r.Id == rotaId);

            if (rota == null)
                return new Result<Unit>(new ArgumentException("Rota não encontrada"));

            // Opcionalmente, poderíamos verificar se já existe subrota com mesmo Código
            // var codigoExiste = rota.Subrotas.Any(s => s.Codigo == subrotaRequest.Codigo);
            // if (codigoExiste) return new Result<Unit>(new ArgumentException("Código de Subrota já existe"));

            var novaSubrota = new Subrota
            {
                RotaId = rota.Id,
                Codigo = subrotaRequest.Codigo
            };

            // Basta adicionar na propriedade de navegação
            rota.Subrotas.Add(novaSubrota);

            await db.SaveChangesAsync();
            return new Result<Unit>();
        }
        catch (Exception e)
        {
            return new Result<Unit>(e);
        }
    }

    public async Task<Result<Unit>> UpdateSubrotaAsync(Guid rotaId, Guid subrotaId, CreateSubrotaRequest subrotaRequest)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync();
            var rota = await db.Rotas
                .Include(r => r.Subrotas)
                .FirstOrDefaultAsync(r => r.Id == rotaId);

            if (rota == null)
                return new Result<Unit>(new ArgumentException("Rota não encontrada"));

            var subrota = rota.Subrotas.FirstOrDefault(s => s.Id == subrotaId);
            if (subrota == null)
                return new Result<Unit>(new ArgumentException("Subrota não encontrada"));

            // Atualiza os campos
            db.Entry(subrota).CurrentValues.SetValues(subrotaRequest);

            await db.SaveChangesAsync();
            return new Result<Unit>();
        }
        catch (Exception e)
        {
            return new Result<Unit>(e);
        }
    }

    public async Task<Result<Unit>> DeleteSubrotaAsync(Guid rotaId, Guid subrotaId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var rota = await db.Rotas
            .Include(r => r.Subrotas)
            .FirstOrDefaultAsync(r => r.Id == rotaId);

        if (rota == null)
            return new Result<Unit>(new ArgumentException("Rota não encontrada"));

        var subrota = rota.Subrotas.FirstOrDefault(s => s.Id == subrotaId);
        if (subrota == null)
            return new Result<Unit>(new ArgumentException("Subrota não encontrada"));

        // Remove da lista de navegação
        rota.Subrotas.Remove(subrota);

        await db.SaveChangesAsync();
        return new Result<Unit>();
    }
}
