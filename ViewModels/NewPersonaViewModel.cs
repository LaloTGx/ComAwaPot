using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class NewPersonaViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    public static Situacion[] Estados { get; } =
        Enum.GetValues<Situacion>();

    public event Func<Task>? PersonaCreada;

    public event Func<Task>? CancelarSolicitado;

    // -------------------------
    // Datos de la persona
    // -------------------------

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string segundoNombre = string.Empty;

    [ObservableProperty]
    private string primerApellido = string.Empty;

    [ObservableProperty]
    private string segundoApellido = string.Empty;

    // -------------------------
    // Datos de la primera toma
    // -------------------------

    [ObservableProperty]
    private int numeroContrato;

    [ObservableProperty]
    private string calle = string.Empty;

    [ObservableProperty]
    private int numExt;

    [ObservableProperty]
    private Situacion estado = Situacion.Activa;

    public NewPersonaViewModel(PersonaService personaService)
    {
        _personaService = personaService;
    }

    [RelayCommand]
    private async Task Guardar()
    {
        // Validación de persona
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        if (string.IsNullOrWhiteSpace(PrimerApellido))
            return;

        if (string.IsNullOrWhiteSpace(SegundoApellido))
            return;

        // Validación de primera toma
        if (NumeroContrato <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Calle))
            return;

        if (NumExt <= 0)
            return;

        var persona = new Persona
        {
            Nombre = Nombre.Trim(),

            SegundoNombre = string.IsNullOrWhiteSpace(SegundoNombre)
                ? null
                : SegundoNombre.Trim(),

            PrimerApellido = PrimerApellido.Trim(),

            SegundoApellido = SegundoApellido.Trim()
        };

        var toma = new Toma
        {
            NumeroContrato = NumeroContrato,
            Calle = Calle.Trim(),
            NumExt = NumExt,
            Estado = Estado
        };

        var creada = await _personaService
            .CrearConPrimeraTomaAsync(persona, toma);

        if (!creada)
            return;

        LimpiarFormulario();

        if (PersonaCreada is not null)
        {
            await PersonaCreada();
        }
    }

    private void LimpiarFormulario()
    {
        Nombre = string.Empty;
        SegundoNombre = string.Empty;
        PrimerApellido = string.Empty;
        SegundoApellido = string.Empty;

        NumeroContrato = 0;
        Calle = string.Empty;
        NumExt = 0;
        Estado = Situacion.Activa;
    }

    [RelayCommand]
	private async Task Cancelar()
	{
	    if (CancelarSolicitado is not null)
	    {
	        await CancelarSolicitado();
	    }
	}
}
