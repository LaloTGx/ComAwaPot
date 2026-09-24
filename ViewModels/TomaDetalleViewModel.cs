using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class TomaDetalleViewModel : ObservableObject
{
    private readonly TomaService _tomaService;

    [ObservableProperty]
    private Toma? toma;

    public event Func<int, Task>? SolicitarPersona;

    public TomaDetalleViewModel(TomaService tomaService)
    {
        _tomaService = tomaService;
    }

    public async Task CargarTomaAsync(int tomaId)
    {
        Toma = await _tomaService.ObtenerPorIdAsync(tomaId);
    }

    [RelayCommand]
    private async Task VerPersona()
    {
        if (Toma?.Persona is null)
            return;

        if (SolicitarPersona is not null)
        {
            await SolicitarPersona(Toma.Persona.PersonaId);
        }
    }
}
