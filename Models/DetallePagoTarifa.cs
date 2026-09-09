namespace ComAwaPot.Models;

public class DetallePagoTarifa
{
    public int IdDetallePagoTarifa { get; set; }

    public int IdPagoTarifa { get; set; }

    public int IdTarifa { get; set; }

    public DateTime Periodo { get; set; }

    public decimal MontoAplicado { get; set; }

    public PagoTarifa PagoTarifa { get; set; } = null!;

    public Tarifa Tarifa { get; set; } = null!;
}
