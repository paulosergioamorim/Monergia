using System.ComponentModel.DataAnnotations;

namespace Monergia.Models;

public class VisitaElevador
{
    public Guid VisitaTecnicaId { get; set; }
    public VisitaTecnica VisitaTecnica { get; set; } = null!;
    
    public Guid ElevadorId { get; set; }
    public Elevador Elevador { get; set; } = null!;
    
    // Campos adicionais específicos para cada elevador na visita
    [MaxLength(255)]
    public string? Observacoes { get; set; }
    public StatusServico Status { get; set; }
}