namespace ComAwaPot.Models;

public class TarifaToma
{
    public int TarifaTomaId { get; set; }

    public int TomaId { get; set; }

    public int TarifaId { get; set; }

    public Toma Toma { get; set; } = null!;

    public Tarifa Tarifa { get; set; } = null!;
}
