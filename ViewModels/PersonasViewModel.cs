using System.Collections.ObjectModel;
using System.Linq;
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class PersonasViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    public ObservableCollection<Persona> Personas { get; } = [];

    public ObservableCollection<Persona> PersonasFiltradas { get; } = [];

    public event Func<int, Task>? SolicitarDetallePersona;

    public event Func<Task>? SolicitarNuevaPersona;

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    [ObservableProperty]
    private Persona? personaSeleccionada;

    public PersonasViewModel(PersonaService personaService)
    {
        _personaService = personaService;
    }

    public async Task CargarPersonasAsync()
    {
        var personas = await _personaService.ObtenerTodasAsync();

        Personas.Clear();

        foreach (var persona in personas)
        {
            Personas.Add(persona);
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

        var personas = string.IsNullOrWhiteSpace(texto)
            ? Personas
            : Personas.Where(p =>
                p.NombreCompleto.Contains(
                    texto,
                    StringComparison.OrdinalIgnoreCase));

        PersonasFiltradas.Clear();

        foreach (var persona in personas)
        {
            PersonasFiltradas.Add(persona);
        }
    }

    [RelayCommand]
    private async Task NuevaPersona()
    {
        if (SolicitarNuevaPersona is not null)
        {
            await SolicitarNuevaPersona();
        }
    }

    [RelayCommand]
    private async Task SeleccionarPersona(Persona persona)
    {
        PersonaSeleccionada = persona;

        if (SolicitarDetallePersona is not null)
        {
            await SolicitarDetallePersona(persona.PersonaId);
        }
    }
}
