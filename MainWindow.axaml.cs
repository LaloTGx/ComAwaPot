using Avalonia.Controls;
using Avalonia.Input;
using ComAwaPot.ViewModels;

namespace ComAwaPot;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CerrarNuevaPersona_Click(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.CerrarNuevaPersona();
        }
    }
}
