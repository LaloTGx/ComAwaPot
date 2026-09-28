using ComAwaPot.Models;
using ComAwaPot.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class EditPersonaViewModel : ObservableObject
{
    private readonly PersonaService _personaService;

    private int _personaId;

    public event Func<Task>? PersonaActualizada;

    public event Func<Task>? CancelarSolicitado;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string segundoNombre = string.Empty;

    [ObservableProperty]
    private string primerApellido = string.Empty;

    [ObservableProperty]
    private string segundoApellido = string.Empty;

    public EditPersonaViewModel(PersonaService personaService)
    {
        _personaService = personaService;
    }

    public void PrepararParaPersona(Persona persona)
    {
        _personaId = persona.PersonaId;

        Nombre = persona.Nombre;
        SegundoNombre = persona.SegundoNombre ?? string.Empty;
        PrimerApellido = persona.PrimerApellido;
        SegundoApellido = persona.SegundoApellido;
    }

    [RelayCommand]
    private async Task Cancelar()
    {
        if (CancelarSolicitado is not null)
        {
            await CancelarSolicitado();
        }
    }

    [RelayCommand]
    private async Task Guardar()
    {
        if (_personaId <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        if (string.IsNullOrWhiteSpace(PrimerApellido))
            return;

        if (string.IsNullOrWhiteSpace(SegundoApellido))
            return;

        var persona = new Persona
        {
            PersonaId = _personaId,
            Nombre = Nombre.Trim(),
            SegundoNombre = string.IsNullOrWhiteSpace(SegundoNombre)
                ? null
                : SegundoNombre.Trim(),
            PrimerApellido = PrimerApellido.Trim(),
            SegundoApellido = SegundoApellido.Trim()
        };

        await _personaService.ActualizarAsync(persona);

        if (PersonaActualizada is not null)
        {
            await PersonaActualizada();
        }
    }
}
