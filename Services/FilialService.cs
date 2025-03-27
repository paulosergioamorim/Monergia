using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Monergia.DbContexts;
using Monergia.Models;
using Monergia.Requests;

namespace Monergia.Services;

public class FilialService : IFilialService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public FilialService(IDbContextFactory<AppDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory;

    public async Task<IEnumerable<Filial>> GetAllFiliaisAsync()
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Filiais.ToListAsync();
    }

    public async Task<Filial?> GetFilialByIdAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Filiais.FindAsync(id);
    }

    public async Task<Filial?> GetFilialByNomeAsync(string nome)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Filiais.SingleOrDefaultAsync(x => x.Name == nome);
    }

    public async Task<Result<Unit>> CreateFilialAsync(CreateFilialRequest filialRequest)
    {
        try
        {
            var filial = new Filial
            {
                Name = filialRequest.Name
            };

            await using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Filiais.AddAsync(filial);
            await db.SaveChangesAsync();

            return new Result<Unit>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<Unit>(e);
        }
    }

    public async Task<Result<Unit>> UpdateFilialByIdAsync(Guid id, CreateFilialRequest filial)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var existingFilial = await db.Filiais.FindAsync(id);

            if (existingFilial == null)
                return new Result<Unit>(new ArgumentException("Filial não existe"));

            db.Entry(existingFilial).CurrentValues.SetValues(filial);
            await db.SaveChangesAsync();

            return new Result<Unit>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Result<Unit>(e);
        }
    }

    public async Task<Result<Unit>> DeleteFilialByIdAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var existingFilial = await db.Filiais.FindAsync(id);

        if (existingFilial == null)
        {
            return new Result<Unit>(new ArgumentException("Filial não existe"));
        }

        db.Filiais.Remove(existingFilial);
        await db.SaveChangesAsync();

        return new Result<Unit>();
    }
}