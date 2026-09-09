namespace ComAwaPot.Models;

public class HistorialSituacion
{
    public int IdHistorialSituacion { get; set; }

    public int IdPersona { get; set; }

    public Situacion Estado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public Persona Persona { get; set; } = null!;
}
