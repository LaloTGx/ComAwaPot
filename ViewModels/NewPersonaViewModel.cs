using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class NewPersonaViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    public event Func<Task>? PersonaCreada;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string segundoNombre = string.Empty;

    [ObservableProperty]
    private string primerApellido = string.Empty;

    [ObservableProperty]
    private string segundoApellido = string.Empty;

    public NewPersonaViewModel(PersonaService personaService)
    {
        _personaService = personaService;
    }

    [RelayCommand]
private async Task Guardar()
{
    if (string.IsNullOrWhiteSpace(Nombre))
        return;

    if (string.IsNullOrWhiteSpace(PrimerApellido))
        return;

    if (string.IsNullOrWhiteSpace(SegundoApellido))
        return;

    var persona = new Persona
    {
        Nombre = Nombre.Trim(),

        SegundoNombre = string.IsNullOrWhiteSpace(SegundoNombre)
            ? null
            : SegundoNombre.Trim(),

        PrimerApellido = PrimerApellido.Trim(),

        SegundoApellido = SegundoApellido.Trim()
    };

    await _personaService.CrearAsync(persona);

    Nombre = string.Empty;
    SegundoNombre = string.Empty;
    PrimerApellido = string.Empty;
    SegundoApellido = string.Empty;

    if (PersonaCreada is not null)
    {
        await PersonaCreada();
    }
}
}
