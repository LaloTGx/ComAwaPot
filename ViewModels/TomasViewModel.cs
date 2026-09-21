using System.Collections.ObjectModel;
using System.Linq;
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class TomasViewModel : ObservableObject
{
    private readonly TomaService _tomaService;

    public ObservableCollection<Toma> Tomas { get; } = [];

    public ObservableCollection<Toma> TomasFiltradas { get; } = [];

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    [ObservableProperty]
    private Toma? tomaSeleccionada;

    public TomasViewModel(TomaService tomaService)
    {
        _tomaService = tomaService;
    }

    public async Task CargarTomasAsync()
    {
        var tomas = await _tomaService.ObtenerTodasAsync();

        Tomas.Clear();

        foreach (var toma in tomas)
        {
            Tomas.Add(toma);
        }

        ActualizarFiltro();
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        ActualizarFiltro();
    }

    private void ActualizarFiltro()
    {
        var texto = TextoBusqueda.Trim();

        var tomas = string.IsNullOrWhiteSpace(texto)
            ? Tomas
            : Tomas.Where(t =>
                t.NumeroContrato.ToString().Contains(texto) ||
                t.Calle.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                t.Persona.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase));

        TomasFiltradas.Clear();

        foreach (var toma in tomas)
        {
            TomasFiltradas.Add(toma);
        }
    }

    partial void OnTomaSeleccionadaChanged(Toma? value)
    {
        // Aquí despues cargaremos el detalle de la toma.
    }

    [RelayCommand]
    private void SeleccionarToma(Toma toma)
    {
        TomaSeleccionada = toma;
    }
}
