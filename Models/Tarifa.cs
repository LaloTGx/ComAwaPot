namespace ComAwaPot.Models;

public class Tarifa
{
    public int TarifaId { get; set; }

    public TipoTarifa Tipo { get; set; }

    public DateTime Periodo { get; set; }

    public decimal MontoMensual { get; set; }

    public ICollection<TarifaToma> Tomas { get; set; }
        = new List<TarifaToma>();
}
