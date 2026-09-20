using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
private readonly PersonasViewModel _personasViewModel;

[ObservableProperty]
private ObservableObject currentViewModel;

public MainWindowViewModel(PersonasViewModel personasViewModel)
{
    _personasViewModel = personasViewModel;

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

}

