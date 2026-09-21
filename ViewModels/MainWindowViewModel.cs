using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComAwaPot.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
private readonly PersonasViewModel _personasViewModel;
private readonly TomasViewModel _tomasViewModel;

[ObservableProperty]
private ObservableObject currentViewModel;

public MainWindowViewModel(PersonasViewModel personasViewModel, TomasViewModel tomasViewModel)
{
    _personasViewModel = personasViewModel;
    _tomasViewModel = tomasViewModel;

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

[RelayCommand]
private async Task MostrarTomas()
{
    await _tomasViewModel.CargarTomasAsync();
    CurrentViewModel = _tomasViewModel;
}
}
