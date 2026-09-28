using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComAwaPot.Models;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly PersonasViewModel _personasViewModel;
    private readonly PersonaDetalleViewModel _personaDetalleViewModel;
    private readonly TomasViewModel _tomasViewModel;
    private readonly NewTomaViewModel _newTomaViewModel;
    private readonly NewPersonaViewModel _newPersonaViewModel;
    private readonly EditPersonaViewModel _editPersonaViewModel;

    [ObservableProperty]
    private ObservableObject currentViewModel;

    [ObservableProperty]
    private bool mostrarNuevaPersona;

    [ObservableProperty]
    private bool mostrarEditarPersona;

    [ObservableProperty]
    private bool mostrarNuevaToma;

    public MainWindowViewModel(
        PersonasViewModel personasViewModel,
        TomasViewModel tomasViewModel,
        PersonaDetalleViewModel personaDetalleViewModel,
        NewTomaViewModel newTomaViewModel,
        NewPersonaViewModel newPersonaViewModel,
        EditPersonaViewModel editPersonaViewModel)
    {
        _personasViewModel = personasViewModel;
        _personaDetalleViewModel = personaDetalleViewModel;
        _tomasViewModel = tomasViewModel;
        _newTomaViewModel = newTomaViewModel;
        _newPersonaViewModel = newPersonaViewModel;
        _editPersonaViewModel = editPersonaViewModel;

        _personasViewModel.SolicitarNuevaPersona += AbrirNuevaPersonaAsync;
        _personasViewModel.SolicitarDetallePersona += MostrarDetallePersonaAsync;

        _tomasViewModel.SolicitarDetallePersona += MostrarDetallePersonaAsync;

        _newTomaViewModel.TomaCreada += VolverAPersonaDetalleAsync;
        _newTomaViewModel.CancelarSolicitado += CerrarNuevaTomaAsync;

        _newPersonaViewModel.PersonaCreada += PersonaCreadaAsync;
        _newPersonaViewModel.CancelarSolicitado += CerrarNuevaPersonaAsync;

        _editPersonaViewModel.PersonaActualizada += PersonaActualizadaAsync;
        _editPersonaViewModel.CancelarSolicitado += CerrarEditarPersonaAsync;

        _personaDetalleViewModel.SolicitarNuevaToma += MostrarNuevaTomaAsync;
        _personaDetalleViewModel.SolicitarEditarPersona += AbrirEditarPersonaAsync;

        currentViewModel = new HomeViewModel();
    }

    public NewPersonaViewModel NewPersonaViewModel =>
        _newPersonaViewModel;

    public EditPersonaViewModel EditPersonaViewModel =>
        _editPersonaViewModel;

    public NewTomaViewModel NewTomaViewModel =>
        _newTomaViewModel;

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

        MostrarNuevaToma = true;

        await Task.CompletedTask;
    }

    private async Task VolverAPersonaDetalleAsync()
    {
        MostrarNuevaToma = false;

        if (_personaDetalleViewModel.Persona is null)
            return;

        await MostrarDetallePersonaAsync(
            _personaDetalleViewModel.Persona.PersonaId);
    }

    private async Task CerrarNuevaTomaAsync()
    {
        MostrarNuevaToma = false;

        await Task.CompletedTask;
    }

    [RelayCommand]
    public void CerrarNuevaToma()
    {
        MostrarNuevaToma = false;
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

    private async Task AbrirEditarPersonaAsync(Persona persona)
    {
        _editPersonaViewModel.PrepararParaPersona(persona);

        MostrarEditarPersona = true;

        await Task.CompletedTask;
    }

    private async Task PersonaActualizadaAsync()
    {
        MostrarEditarPersona = false;

        if (_personaDetalleViewModel.Persona is null)
            return;

        await MostrarDetallePersonaAsync(
            _personaDetalleViewModel.Persona.PersonaId);
    }

    private async Task CerrarEditarPersonaAsync()
    {
        MostrarEditarPersona = false;

        await Task.CompletedTask;
    }

    [RelayCommand]
    public void CerrarEditarPersona()
    {
        MostrarEditarPersona = false;
    }
}
