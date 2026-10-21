
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ComAwaPot.ViewModels;

public partial class TarifasViewModel : ObservableObject
{
    private readonly TarifaService _tarifaService;

    public ObservableCollection<Tarifa> Tarifas { get; } = new();

    public static TipoTarifa[] Tipos { get; } =
        Enum.GetValues<TipoTarifa>();

    [ObservableProperty]
    private TipoTarifa tipo = TipoTarifa.Domestica;

    [ObservableProperty]
    private DateTime periodo = new(
        DateTime.Today.Year,
        DateTime.Today.Month,
        1);

    [ObservableProperty]
    private decimal montoMensual;

    [ObservableProperty]
    private bool mostrandoFormulario;

    [ObservableProperty]
    private string mensaje = string.Empty;

    public TarifasViewModel(TarifaService tarifaService)
    {
        _tarifaService = tarifaService;
    }

    [RelayCommand]
    public async Task CargarTarifasAsync()
    {
        var tarifas = await _tarifaService.ObtenerTodasAsync();

        Tarifas.Clear();

        foreach (var tarifa in tarifas)
        {
            Tarifas.Add(tarifa);
        }
    }

    [RelayCommand]
    private void NuevaTarifa()
    {
        Tipo = TipoTarifa.Domestica;
        Periodo = new DateTime(
            DateTime.Today.Year,
            DateTime.Today.Month,
            1);
        MontoMensual = 0;
        Mensaje = string.Empty;
        MostrandoFormulario = true;
    }

    [RelayCommand]
    private void Cancelar()
    {
        MostrandoFormulario = false;
        Mensaje = string.Empty;
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (MontoMensual <= 0)
        {
            Mensaje = "El monto mensual debe ser mayor que cero.";
            return;
        }

        var numeroMes = Array.IndexOf(Meses, MesSeleccionado) + 1;

		if (numeroMes < 1)
		{
		    Mensaje = "Selecciona un mes válido.";
		    return;
		}

		var tarifa = new Tarifa
		{
		    Tipo = Tipo,
		    Periodo = new DateTime(AnioSeleccionado, numeroMes, 1),
		    MontoMensual = MontoMensual
		};

        var creada = await _tarifaService.CrearAsync(tarifa);

        if (creada is null)
        {
            Mensaje = "Ya existe una tarifa de ese tipo para ese periodo.";
            return;
        }

        Tarifas.Add(creada);

        var tarifasOrdenadas = Tarifas
            .OrderBy(t => t.Periodo)
            .ThenBy(t => t.Tipo)
            .ToList();

        Tarifas.Clear();

        foreach (var item in tarifasOrdenadas)
        {
            Tarifas.Add(item);
        }

        MostrandoFormulario = false;
        Mensaje = string.Empty;
    }

    public string[] Meses { get; } =
	{
	    "Enero", "Febrero", "Marzo", "Abril",
	    "Mayo", "Junio", "Julio", "Agosto",
	    "Septiembre", "Octubre", "Noviembre", "Diciembre"
	};

	public int[] Anios { get; } =
	    Enumerable.Range(DateTime.Today.Year - 2, 8).ToArray();

	[ObservableProperty]
	private string mesSeleccionado =
	    DateTime.Today.ToString("MMMM",
	        new System.Globalization.CultureInfo("es-MX"));

	[ObservableProperty]
	private int anioSeleccionado = DateTime.Today.Year;
}

