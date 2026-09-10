namespace ComAwaPot.Models;

public class HistorialSituacion
{
    public int HistorialSituacionId { get; set; }

    public int PersonaId { get; set; }

    public Situacion Estado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public Persona Persona { get; set; } = null!;
}
