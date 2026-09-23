namespace ComAwaPot.Models;

public class Persona
{
    public int PersonaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = string.Empty;

    public string SegundoApellido { get; set; } = string.Empty;

    public string NombreCompleto
    {
        get
        {
            return string.Join(
                " ",
                new[]
                {
                    Nombre,
                    SegundoNombre,
                    PrimerApellido,
                    SegundoApellido
                }
                .Where(x => !string.IsNullOrWhiteSpace(x))
            );
        }
    }

    public ICollection<Toma> Tomas { get; set; }
        = new List<Toma>();

    public ICollection<PagoAportacion> PagosAportacion { get; set; }
        = new List<PagoAportacion>();

    public ICollection<AportacionPersona> Aportaciones { get; set; }
        = new List<AportacionPersona>();
}
