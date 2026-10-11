using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class EditTomaViewModel : ObservableObject
{
    private readonly TomaService _tomaService;

    private int _tomaId;

    public static Situacion[] Estados { get; } =
        Enum.GetValues<Situacion>();

    public event Func<Task>? TomaActualizada;

    public event Func<Task>? CancelarSolicitado;

    [ObservableProperty]
    private int numeroContrato;

    [ObservableProperty]
    private string calle = string.Empty;

    [ObservableProperty]
    private int numExt;

    [ObservableProperty]
    private Situacion estado = Situacion.Activa;

    [ObservableProperty]
    private string mensajeError = string.Empty;

    public EditTomaViewModel(TomaService tomaService)
    {
        _tomaService = tomaService;
    }

    public void PrepararParaToma(Toma toma)
    {
        _tomaId = toma.TomaId;

        NumeroContrato = toma.NumeroContrato;
        Calle = toma.Calle;
        NumExt = toma.NumExt;
        Estado = toma.Estado;

        MensajeError = string.Empty;
    }

    [RelayCommand]
    private async Task Cancelar()
    {
        if (CancelarSolicitado is not null)
            await CancelarSolicitado();
    }

    [RelayCommand]
    private async Task Guardar()
    {
        MensajeError = string.Empty;

        if (_tomaId <= 0)
        {
            MensajeError = "No se ha seleccionado una toma válida.";
            return;
        }

        if (NumeroContrato <= 0)
        {
            MensajeError = "Ingresa un número de contrato válido.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Calle))
        {
            MensajeError = "Ingresa la calle.";
            return;
        }

        if (NumExt <= 0)
        {
            MensajeError = "Ingresa un número válido.";
            return;
        }

        var toma = new Toma
        {
            TomaId = _tomaId,
            NumeroContrato = NumeroContrato,
            Calle = Calle.Trim(),
            NumExt = NumExt
        };

        var actualizada = await _tomaService.ActualizarCompletaAsync(
            toma,
            Estado,
            DateTime.Today);

        if (!actualizada)
        {
            MensajeError =
                "No se pudieron guardar los cambios. " +
                "Verifica que el contrato no esté registrado " +
                "y vuelve a intentarlo.";
            return;
        }

        if (TomaActualizada is not null)
            await TomaActualizada();
    }
}
