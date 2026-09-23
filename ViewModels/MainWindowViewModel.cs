using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
private readonly PersonasViewModel _personasViewModel;
private readonly TomasViewModel _tomasViewModel;
private readonly NewPersonaViewModel _newPersonaViewModel;
private readonly PersonaDetalleViewModel _personaDetalleViewModel;

[ObservableProperty]
private ObservableObject currentViewModel;

public MainWindowViewModel(
        PersonasViewModel personasViewModel,
        TomasViewModel tomasViewModel,
        PersonaDetalleViewModel personaDetalleViewModel,
        NewPersonaViewModel newPersonaViewModel
        )
{
    _personasViewModel = personasViewModel;
    _tomasViewModel = tomasViewModel;
    _personaDetalleViewModel = personaDetalleViewModel;
    _newPersonaViewModel = newPersonaViewModel;

    _personasViewModel.SolicitarNuevaPersona += MostrarNuevaPersonaAsync;
    _newPersonaViewModel.PersonaCreada += VolverAPersonasAsync;
    _personasViewModel.SolicitarDetallePersona += MostrarDetallePersonaAsync;

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

}
