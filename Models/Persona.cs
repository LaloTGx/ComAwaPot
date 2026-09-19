namespace ComAwaPot.Models;

public class Persona
{
    public int PersonaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public ICollection<Toma> Tomas { get; set; }
        = new List<Toma>();

    public ICollection<PagoAportacion> PagosAportacion { get; set; }
        = new List<PagoAportacion>();

    public ICollection<AportacionPersona> Aportaciones { get; set; }
    = new List<AportacionPersona>();
}
