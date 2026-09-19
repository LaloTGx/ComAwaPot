namespace ComAwaPot.Models;

public class AportacionPersona
{
    public int AportacionPersonaId { get; set; }

    public int AportacionExtraordinariaId { get; set; }

    public int PersonaId { get; set; }

    public decimal MontoEsperado { get; set; }

    public AportacionExtraordinaria AportacionExtraordinaria { get; set; } = null!;

    public Persona Persona { get; set; } = null!;
}
