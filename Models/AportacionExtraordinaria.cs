namespace ComAwaPot.Models;

public class AportacionExtraordinaria
{
    public int AportacionExtraordinariaId { get; set; }

    public string Concepto { get; set; } = string.Empty;

    public decimal MontoTotal { get; set; }

    public decimal MontoPorPersona { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaLimite { get; set; }

    public string? Observaciones { get; set; }

    public ICollection<PagoAportacion> Pagos { get; set; }
        = new List<PagoAportacion>();
}
