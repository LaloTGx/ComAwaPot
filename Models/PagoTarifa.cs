namespace ComAwaPot.Models;

public class PagoTarifa
{
    public int IdPagoTarifa { get; set; }

    public int IdPersona { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal MontoTotal { get; set; }

    public Persona Persona { get; set; } = null!;

    public ICollection<DetallePagoTarifa> Detalles { get; set; }
        = new List<DetallePagoTarifa>();
}
