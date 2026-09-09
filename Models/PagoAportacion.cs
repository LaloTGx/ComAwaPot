namespace ComAwaPot.Models;

public class PagoAportacion
{
    public int IdPagoAportacion { get; set; }

    public int IdAportacionExtraordinaria { get; set; }

    public int IdPersona { get; set; }

    public decimal MontoPagado { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Observaciones { get; set; }

    public AportacionExtraordinaria AportacionExtraordinaria { get; set; } = null!;

    public Persona Persona { get; set; } = null!;
}
