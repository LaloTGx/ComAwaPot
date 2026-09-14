namespace ComAwaPot.Models;

public class PagoTarifa
{
    public int PagoTarifaId { get; set; }

    public int TomaId { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal MontoTotal { get; set; }

    public Toma Toma { get; set; } = null!;

    public ICollection<DetallePagoTarifa> Detalles { get; set; }
        = new List<DetallePagoTarifa>();
}
