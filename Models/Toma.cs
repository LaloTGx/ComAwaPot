namespace ComAwaPot.Models;

public class Toma
{
    public int TomaId { get; set; }

    public int NumeroContrato { get; set; }

    public int PersonaId { get; set; }

    public string Calle { get; set; } = string.Empty;

    public int NumExt { get; set; }

    public Situacion Estado { get; set; }

    public Persona Persona { get; set; } = null!;

    public ICollection<HistorialSituacion> HistorialSituaciones { get; set; }
        = new List<HistorialSituacion>();

    public ICollection<PagoTarifa> PagosTarifa { get; set; }
        = new List<PagoTarifa>();

    public ICollection<TarifaToma> Tarifas { get; set; }
        = new List<TarifaToma>();
}
