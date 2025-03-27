using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Monergia.Models;

namespace Monergia.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Acesso> Acessos { get; set; }
    public DbSet<Condominio> Condominios { get; set; }
    public DbSet<Elevador> Elevadores { get; set; }
    public DbSet<Rota> Rotas { get; set; }
    public DbSet<User> Usuarios { get; set; }
    public DbSet<VisitaTecnica> VisitasTecnicas { get; set; }
    public DbSet<VisitaElevador> VisitasElevadores { get; set; }
    public DbSet<Filial> Filiais { get; set; }
    public DbSet<Subrota> Subrotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VisitaTecnica>(e =>
        {
            e.HasOne<User>(p => p.Tecnico1);
            e.HasOne<User>(p => p.Tecnico2);
        });
        
        modelBuilder.Entity<VisitaElevador>()
            .HasKey(ve => new { ve.VisitaTecnicaId, ve.ElevadorId });
    }
}