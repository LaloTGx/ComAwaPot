namespace ComAwaPot.Models;

public class DetallePagoTarifa
{
    public int DetallePagoTarifaId { get; set; }

    public int PagoTarifaId { get; set; }

    public int TarifaId { get; set; }

    public DateTime Periodo { get; set; }

    public decimal MontoAplicado { get; set; }

    public PagoTarifa PagoTarifa { get; set; } = null!;

    public Tarifa Tarifa { get; set; } = null!;
}
