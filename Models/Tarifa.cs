namespace ComAwaPot.Models;

public class Tarifa
{
    public int IdTarifa { get; set; }

    public DateTime Periodo { get; set; }

    public decimal MontoMensual { get; set; }

    public string? Observaciones { get; set; }
}
