using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
private readonly PersonasViewModel _personasViewModel;
private readonly NewPersonaViewModel _newPersonaViewModel;
private readonly PersonaDetalleViewModel _personaDetalleViewModel;
private readonly TomasViewModel _tomasViewModel;
private readonly NewTomaViewModel _newTomaViewModel;
private readonly TomaDetalleViewModel _tomaDetalleViewModel;

[ObservableProperty]
private ObservableObject currentViewModel;

public MainWindowViewModel(
        PersonasViewModel personasViewModel,
        TomasViewModel tomasViewModel,
        PersonaDetalleViewModel personaDetalleViewModel,
        NewPersonaViewModel newPersonaViewModel,
        NewTomaViewModel newTomaViewModel,
        TomaDetalleViewModel tomaDetalleViewModel
        )
{
    _personasViewModel = personasViewModel;
    _tomasViewModel = tomasViewModel;
    _personaDetalleViewModel = personaDetalleViewModel;
    _newPersonaViewModel = newPersonaViewModel;
    _newTomaViewModel = newTomaViewModel;
    _tomaDetalleViewModel = tomaDetalleViewModel;

    _personasViewModel.SolicitarNuevaPersona += MostrarNuevaPersonaAsync;
    _newPersonaViewModel.PersonaCreada += VolverAPersonasAsync;
    _personasViewModel.SolicitarDetallePersona += MostrarDetallePersonaAsync;

    _personaDetalleViewModel.SolicitarNuevaToma += MostrarNuevaTomaAsync;
    _newTomaViewModel.TomaCreada += VolverAPersonaDetalleAsync;
    _personaDetalleViewModel.SolicitarDetalleToma += MostrarDetalleTomaAsync;

    _tomaDetalleViewModel.SolicitarPersona += MostrarDetallePersonaAsync;
    _tomasViewModel.SolicitarDetalleToma += MostrarDetalleTomaAsync;

    currentViewModel = new HomeViewModel();
}

[RelayCommand]
private void MostrarInicio()
{
    CurrentViewModel = new HomeViewModel();
}

[RelayCommand]
private async Task MostrarPersonas()
{
    await _personasViewModel.CargarPersonasAsync();

    CurrentViewModel = _personasViewModel;
}

[RelayCommand]
private void MostrarNuevaPersona()
{
    CurrentViewModel = _newPersonaViewModel;
}

public async Task MostrarDetallePersonaAsync(int personaId)
{
    await _personaDetalleViewModel.CargarPersonaAsync(personaId);

    CurrentViewModel = _personaDetalleViewModel;
}

[RelayCommand]
private async Task MostrarTomas()
{
    await _tomasViewModel.CargarTomasAsync();
    CurrentViewModel = _tomasViewModel;
}

private Task MostrarNuevaPersonaAsync()
{
    CurrentViewModel = _newPersonaViewModel;

    return Task.CompletedTask;
}

private async Task VolverAPersonasAsync()
{
    await _personasViewModel.CargarPersonasAsync();

    CurrentViewModel = _personasViewModel;
}

private async Task MostrarNuevaTomaAsync(int personaId)
{
    _newTomaViewModel.PrepararParaPersona(personaId);

    CurrentViewModel = _newTomaViewModel;

    await Task.CompletedTask;
}

private async Task VolverAPersonaDetalleAsync()
{
    if (_personaDetalleViewModel.Persona is null)
        return;

    await _personaDetalleViewModel.CargarPersonaAsync(
        _personaDetalleViewModel.Persona.PersonaId);

    CurrentViewModel = _personaDetalleViewModel;
}

private async Task MostrarDetalleTomaAsync(int tomaId)
{
    await _tomaDetalleViewModel.CargarTomaAsync(tomaId);

    CurrentViewModel = _tomaDetalleViewModel;
}

}
