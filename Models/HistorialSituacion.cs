namespace ComAwaPot.Models;

public class HistorialSituacion
{
    public int HistorialSituacionId { get; set; }

    public int TomaId { get; set; }

    public Situacion Estado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public Toma Toma { get; set; } = null!;
}
