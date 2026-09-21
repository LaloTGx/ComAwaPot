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

    [ObservableProperty]
    private string nuevoNombre = string.Empty;

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    [ObservableProperty]
    private Persona? personaSeleccionada;

    [ObservableProperty]
    private string nombreEdicion = string.Empty;

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

    partial void OnPersonaSeleccionadaChanged(
        Persona? value)
    {
        NombreEdicion = value?.Nombre ?? string.Empty;
    }

    private void ActualizarFiltro()
    {
        var texto = TextoBusqueda.Trim();

        var personas = string.IsNullOrWhiteSpace(texto)
            ? Personas
            : Personas.Where(p =>
                p.Nombre.Contains(
                    texto,
                    StringComparison.OrdinalIgnoreCase));

        PersonasFiltradas.Clear();

        foreach (var persona in personas)
        {
            PersonasFiltradas.Add(persona);
        }
    }

    [RelayCommand]
    private async Task AgregarPersona()
    {
        if (string.IsNullOrWhiteSpace(NuevoNombre))
            return;

        var persona = new Persona
        {
            Nombre = NuevoNombre.Trim()
        };

        var personaCreada =
            await _personaService.CrearAsync(persona);

        Personas.Add(personaCreada);

        NuevoNombre = string.Empty;

        ActualizarFiltro();
    }

    [RelayCommand]
    private async Task GuardarEdicion()
    {
        if (PersonaSeleccionada is null)
            return;

        if (string.IsNullOrWhiteSpace(NombreEdicion))
            return;

        PersonaSeleccionada.Nombre = NombreEdicion.Trim();

        await _personaService.ActualizarAsync(PersonaSeleccionada);

        ActualizarFiltro();
    }

    [RelayCommand]
	private void SeleccionarPersona(Persona persona)
	{
	    PersonaSeleccionada = persona;
	}
}
