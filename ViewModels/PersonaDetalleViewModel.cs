using System.Collections.ObjectModel;
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class PersonaDetalleViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    [ObservableProperty]
    private Persona? persona;

    public ObservableCollection<Toma> Tomas { get; } = [];

    public event Func<int, Task>? SolicitarNuevaToma;

    public event Func<Persona, Task>? SolicitarEditarPersona;

    public PersonaDetalleViewModel(PersonaService personaService)
    {
        _personaService = personaService;
    }

    public async Task CargarPersonaAsync(int personaId)
    {
        var personaEncontrada =
            await _personaService.ObtenerPorIdAsync(personaId);

        Persona = personaEncontrada;

        Tomas.Clear();

        if (personaEncontrada is null)
            return;

        foreach (var toma in personaEncontrada.Tomas)
        {
            Tomas.Add(toma);
        }
    }

    [RelayCommand]
    private async Task NuevaToma()
    {
        if (Persona is null)
            return;

        if (SolicitarNuevaToma is not null)
        {
            await SolicitarNuevaToma(Persona.PersonaId);
        }
    }

    [RelayCommand]
    private async Task EditarPersona()
    {
        if (Persona is null)
            return;

        if (SolicitarEditarPersona is not null)
        {
            await SolicitarEditarPersona(Persona);
        }
    }
}
