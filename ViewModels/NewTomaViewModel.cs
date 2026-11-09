using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TipoTarifaEnum = ComAwaPot.Models.TipoTarifa;

namespace ComAwaPot.ViewModels;

public partial class NewTomaViewModel : ObservableObject
{
    private readonly TomaService _tomaService;

    private int _personaId;

    public static Situacion[] Estados { get; } =
        Enum.GetValues<Situacion>();

    public event Func<Task>? TomaCreada;

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
    private TipoTarifaEnum tipoTarifa = TipoTarifaEnum.Domestica;

    public NewTomaViewModel(TomaService tomaService)
    {
        _tomaService = tomaService;
    }

    public async Task PrepararParaPersonaAsync(int personaId)
    {
        _personaId = personaId;

        NumeroContrato =
            await _tomaService.ObtenerSiguienteNumeroContratoAsync();
        Calle = string.Empty;
        NumExt = 0;
        Estado = Situacion.Activa;
        TipoTarifa = TipoTarifa.Domestica;
    }

    [RelayCommand]
	private void AlternarTipoTarifa()
	{
	    TipoTarifa = TipoTarifa == TipoTarifaEnum.Domestica
	        ? TipoTarifaEnum.Comercial
	        : TipoTarifaEnum.Domestica;
	}

    [RelayCommand]
    private async Task Cancelar()
    {
        if (CancelarSolicitado is not null)
        {
            await CancelarSolicitado();
        }
    }

    [RelayCommand]
    private async Task Guardar()
    {
        if (_personaId <= 0)
            return;

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
            Estado = Estado,
            TipoTarifa = TipoTarifa
        };

        var tomaCreada = await _tomaService.CrearAsync(toma);

        if (tomaCreada is null)
            return;

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
