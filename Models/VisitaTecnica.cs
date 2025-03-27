using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class VisitaTecnica
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateOnly DataVisita { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFim { get; set; }

    public TimeSpan Duracao => HoraFim - HoraInicio;

    public Guid CondominioId { get; set; }

    public Condominio Condominio { get; set; } = null!;

    public Guid Tecnico1Id { get; set; }

    public User Tecnico1 { get; set; } = null!;

    public Guid? Tecnico2Id { get; set; }

    public User? Tecnico2 { get; set; }

    [MaxLength(100)] public string NomeRecebedor { get; set; } = string.Empty;

    [MaxLength(14)] public string DocumentoRecebedor { get; set; } = string.Empty; // CPF/CNPJ

    // Controle de materiais e chaves
    public bool EmpregoMaterial { get; set; }
    public bool DevolucaoChave { get; set; }
    public TimeSpan TempoParalizacao { get; set; }

    // Documentação técnica
    [MaxLength(255)] public string? InformacaoTecnica { get; set; }

    [StringLength(500)] public string[] Imagens { get; set; } = []; // URLs das imagens

    // Relacionamento N:N com Elevador
    public List<VisitaElevador> ElevadoresVisita { get; set; } = [];

    // Tipo de visita (novo campo)
    public TipoVisita Tipo { get; set; }
}