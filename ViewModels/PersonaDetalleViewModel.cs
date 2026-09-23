using System.Collections.ObjectModel;
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ComAwaPot.ViewModels;

public partial class PersonaDetalleViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    [ObservableProperty]
    private Persona? persona;

    public ObservableCollection<Toma> Tomas { get; } = [];

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
}
