using System.Collections.ObjectModel;
using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ComAwaPot.ViewModels;

public partial class PersonasViewModel : ObservableObject
{
private readonly PersonaService _personaService;

public ObservableCollection<Persona> Personas { get; } = [];

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
}

}

