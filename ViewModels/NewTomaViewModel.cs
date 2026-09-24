using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class NewTomaViewModel : ObservableObject
{
    private readonly TomaService _tomaService;

    private int _personaId;

    public static Situacion[] Estados { get; } =
        Enum.GetValues<Situacion>();

    [ObservableProperty]
    private int numeroContrato;

    [ObservableProperty]
    private string calle = string.Empty;

    [ObservableProperty]
    private int numExt;

    [ObservableProperty]
    private Situacion estado = Situacion.Activa;

    public event Func<Task>? TomaCreada;

    public NewTomaViewModel(TomaService tomaService)
    {
        _tomaService = tomaService;
    }

    public void PrepararParaPersona(int personaId)
    {
        _personaId = personaId;

        NumeroContrato = 0;
        Calle = string.Empty;
        NumExt = 0;
        Estado = Situacion.Activa;
    }

    [RelayCommand]
    private async Task Guardar()
    {
        if (NumeroContrato <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Calle))
            return;

        if (NumExt <= 0)
            return;

        var toma = new Toma
        {
            NumeroContrato = NumeroContrato,
            PersonaId = _personaId,
            Calle = Calle.Trim(),
            NumExt = NumExt,
            Estado = Estado
        };

        var tomaCreada = await _tomaService.CrearAsync(toma);

        if (tomaCreada is null)
        {
            // Posteriormente aquí podremos mostrar
            // una notificación de error.
            return;
        }

        NumeroContrato = 0;
        Calle = string.Empty;
        NumExt = 0;
        Estado = Situacion.Activa;

        if (TomaCreada is not null)
        {
            await TomaCreada();
        }
    }
}
