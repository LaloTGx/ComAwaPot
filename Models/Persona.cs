namespace ComAwaPot.Models;

public class Persona
{
    public int IdPersona { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Calle { get; set; } = string.Empty;

    public int NumExt { get; set; }

    public Situacion Estado { get; set; }

    public ICollection<HistorialSituacion> HistorialSituaciones { get; set; }
        = new List<HistorialSituacion>();

    public ICollection<PagoTarifa> PagosTarifa { get; set; }
        = new List<PagoTarifa>();

    public ICollection<PagoAportacion> PagosAportacion { get; set; }
        = new List<PagoAportacion>();
}
