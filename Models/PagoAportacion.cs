namespace ComAwaPot.Models;

public class PagoAportacion
{
    public int PagoAportacionId { get; set; }

    public int AportacionExtraordinariaId { get; set; }

    public int PersonaId { get; set; }

    public decimal MontoPagado { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Observaciones { get; set; }

    public AportacionExtraordinaria AportacionExtraordinaria { get; set; } = null!;

    public Persona Persona { get; set; } = null!;
}
