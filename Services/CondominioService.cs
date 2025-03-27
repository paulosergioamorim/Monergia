using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Monergia.DbContexts;
using Monergia.Models;
using Monergia.Requests;
using Monergia.Response;

namespace Monergia.Services;

public class CondominioService : ICondominioService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IPasswordHasher<Acesso> _passwordHasher;

    public CondominioService(IDbContextFactory<AppDbContext> dbContextFactory, IPasswordHasher<Acesso> passwordHasher)
    {
        _dbContextFactory = dbContextFactory;
        _passwordHasher = passwordHasher;
    }

    // -------------------------------
    //  CREATE
    // -------------------------------
    public async Task<Result<CondominioResponse>> CreateCondominioAsync(CreateCondominioRequest request)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();

            // Carregar a subrota (já com a Rota) para validação
            var subrota = await db.Subrotas
                .Include(s => s.Rota)
                .FirstOrDefaultAsync(s => s.Id == request.SubrotaId);

            if (subrota == null)
                return new Result<CondominioResponse>(new ArgumentException("Subrota inválida"));

            if (request.Acessos.Count() > 3)
                return new Result<CondominioResponse>(new ArgumentException("Máximo 3 acessos permitidos."));

            // Monta entidade Condomínio
            var condominio = new Condominio
            {
                CNPJ = request.CNPJ,
                CPF = request.CPF,
                Name = request.Name,
                Observations = request.Observations,
                TelefonesSincico = request.TelefonesSindico,
                TelefoneAdmin = request.TelefoneAdmin,
                SubrotaId = subrota.Id,
                Subrota = subrota, // associamos subrota para já manter em memória

                CEP = request.CEP,
                Logradouro = request.Logradouro,
                Bairro = request.Bairro,
                Municipio = request.Municipio,
                Estado = request.Estado,
                Numero = request.Numero,
                // Monta Elevadores
                Elevadores = request.Elevadores
                    .Select(e => new Elevador
                    {
                        Nomenclatura = e.Nomenclatura
                    })
                    .ToList()
            };

            foreach (var a in request.Acessos)
            {
                var acesso = new Acesso
                {
                    CPF = a.CPF,
                    CNPJ = a.CNPJ,
                    Name = a.Name
                };

                acesso.PasswordHash = _passwordHasher.HashPassword(acesso, a.Password);
                
                condominio.Acessos.Add(acesso);
            }
            
            await db.Condominios.AddAsync(condominio);
            await db.SaveChangesAsync();

            // Carrega novamente (Include Subrota+Rota) para montar response
            var created = await db.Condominios
                .Include(c => c.Subrota)
                    .ThenInclude(s => s.Rota)
                .Include(c => c.Acessos)
                .Include(c => c.Elevadores)
                .FirstAsync(c => c.Id == condominio.Id);

            return new CondominioResponse(created);
        }
        catch (Exception ex)
        {
            return new Result<CondominioResponse>(ex);
        }
    }

    // -------------------------------
    //  UPDATE
    // -------------------------------
    public async Task<Result<CondominioResponse>> UpdateCondominioAsync(Guid condominioId, CreateCondominioRequest request)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();

            var condominio = await db.Condominios
                .Include(c => c.Subrota)
                .Include(c => c.Acessos)
                .Include(c => c.Elevadores)
                .FirstOrDefaultAsync(c => c.Id == condominioId);

            if (condominio == null)
                return new Result<CondominioResponse>(new ArgumentException("Condomínio não encontrado"));

            // Subrota
            var subrota = await db.Subrotas
                .Include(s => s.Rota)
                .FirstOrDefaultAsync(s => s.Id == request.SubrotaId);

            if (subrota == null)
                return new Result<CondominioResponse>(new ArgumentException("Subrota inválida"));

            // Verifica limite de acessos
            if (request.Acessos.Count() > 3)
                return new Result<CondominioResponse>(new ArgumentException("Máximo 3 acessos permitidos."));
            
            // Atualiza dados principais
            condominio.CNPJ = request.CNPJ;
            condominio.CPF = request.CPF;
            condominio.Name = request.Name;
            condominio.Observations = request.Observations;
            condominio.TelefonesSincico = request.TelefonesSindico;
            condominio.TelefoneAdmin = request.TelefoneAdmin;
            condominio.SubrotaId = subrota.Id;
            condominio.Subrota = subrota;
            condominio.CEP = request.CEP;
            condominio.Logradouro = request.Logradouro;
            condominio.Bairro = request.Bairro;
            condominio.Municipio = request.Municipio;
            condominio.Estado = request.Estado;
            condominio.Numero = request.Numero;

            // Atualiza acessos
            // 1) Remove todos antigos
            condominio.Acessos.Clear();
            // 2) Adiciona novos
            foreach (var a in request.Acessos)
            {
                var acesso = new Acesso
                {
                    CPF = a.CPF,
                    CNPJ = a.CNPJ,
                    Name = a.Name
                };

                acesso.PasswordHash = _passwordHasher.HashPassword(acesso, a.Password);
                
                condominio.Acessos.Add(acesso);
            }

            // Atualiza elevadores
            condominio.Elevadores.Clear();
            foreach (var e in request.Elevadores)
            {
                condominio.Elevadores.Add(new Elevador
                {
                    Nomenclatura = e.Nomenclatura
                });
            }

            await db.SaveChangesAsync();

            // Carrega novamente (Include Subrota+Rota) para montar response
            var updated = await db.Condominios
                .Include(c => c.Subrota)
                    .ThenInclude(s => s.Rota)
                .Include(c => c.Acessos)
                .Include(c => c.Elevadores)
                .FirstAsync(c => c.Id == condominio.Id);

            return new CondominioResponse(updated);
        }
        catch (Exception ex)
        {
            return new Result<CondominioResponse>(ex);
        }
    }

    // -------------------------------
    //  DELETE
    // -------------------------------
    public async Task<Result<Unit>> DeleteCondominioAsync(Guid condominioId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        var condominio = await db.Condominios.FindAsync(condominioId);
        if (condominio == null)
            return new Result<Unit>(new ArgumentException("Condomínio não encontrado"));

        db.Condominios.Remove(condominio);
        await db.SaveChangesAsync();
        return new Result<Unit>();
    }

    // -------------------------------
    //  GET (por ID)
    // -------------------------------
    public async Task<CondominioResponse?> GetCondominioByIdAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var condominio = await db.Condominios
            .Include(c => c.Subrota)
                .ThenInclude(s => s.Rota)
            .Include(c => c.Acessos)
            .Include(c => c.Elevadores)
            .FirstOrDefaultAsync(c => c.Id == id);

        return condominio == null 
            ? null 
            : new CondominioResponse(condominio);
    }

    // -------------------------------
    //  GET (todos)
    // -------------------------------
    public async Task<IEnumerable<CondominioResponse>> GetAllCondominiosAsync()
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var lista = await db.Condominios
            .Include(c => c.Subrota)
                .ThenInclude(s => s.Rota)
            .Include(c => c.Acessos)
            .Include(c => c.Elevadores)
            .ToListAsync();

        return lista.Select(c => new CondominioResponse(c));
    }
}
