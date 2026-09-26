using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly PersonasViewModel _personasViewModel;
    private readonly PersonaDetalleViewModel _personaDetalleViewModel;
    private readonly TomasViewModel _tomasViewModel;
    private readonly NewTomaViewModel _newTomaViewModel;
    private readonly TomaDetalleViewModel _tomaDetalleViewModel;
    private readonly NewPersonaViewModel _newPersonaViewModel;

    [ObservableProperty]
    private ObservableObject currentViewModel;

    [ObservableProperty]
    private bool mostrarNuevaPersona;

    public MainWindowViewModel(
        PersonasViewModel personasViewModel,
        TomasViewModel tomasViewModel,
        PersonaDetalleViewModel personaDetalleViewModel,
        NewTomaViewModel newTomaViewModel,
        TomaDetalleViewModel tomaDetalleViewModel,
        NewPersonaViewModel newPersonaViewModel)
    {
        _personasViewModel = personasViewModel;
        _personaDetalleViewModel = personaDetalleViewModel;
        _tomasViewModel = tomasViewModel;
        _newTomaViewModel = newTomaViewModel;
        _tomaDetalleViewModel = tomaDetalleViewModel;
        _newPersonaViewModel = newPersonaViewModel;

        _personasViewModel.SolicitarNuevaPersona += AbrirNuevaPersonaAsync;
        _personasViewModel.SolicitarDetallePersona += MostrarDetallePersonaAsync;

        _personaDetalleViewModel.SolicitarNuevaToma += MostrarNuevaTomaAsync;
        _newTomaViewModel.TomaCreada += VolverAPersonaDetalleAsync;

        _personaDetalleViewModel.SolicitarDetalleToma += MostrarDetalleTomaAsync;

        _tomaDetalleViewModel.SolicitarPersona += MostrarDetallePersonaAsync;

        _tomasViewModel.SolicitarDetalleToma += MostrarDetalleTomaAsync;

        _newPersonaViewModel.PersonaCreada += PersonaCreadaAsync;
        _newPersonaViewModel.CancelarSolicitado += CerrarNuevaPersonaAsync;

        currentViewModel = new HomeViewModel();
    }

    public NewPersonaViewModel NewPersonaViewModel =>
        _newPersonaViewModel;

    [RelayCommand]
    private void MostrarInicio()
    {
        CurrentViewModel = new HomeViewModel();
    }

    [RelayCommand]
    public async Task MostrarPersonas()
    {
        await _personasViewModel.CargarPersonasAsync();

        CurrentViewModel = _personasViewModel;
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

    private async Task AbrirNuevaPersonaAsync()
    {
        MostrarNuevaPersona = true;

        await Task.CompletedTask;
    }

    private async Task PersonaCreadaAsync()
    {
        MostrarNuevaPersona = false;

        await MostrarPersonas();
    }

    private async Task CerrarNuevaPersonaAsync()
    {
        MostrarNuevaPersona = false;

        await Task.CompletedTask;
    }

    [RelayCommand]
    public void CerrarNuevaPersona()
    {
        MostrarNuevaPersona = false;
    }
}
