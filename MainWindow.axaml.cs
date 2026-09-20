using Avalonia.Controls;
using ComAwaPot.ViewModels;

namespace ComAwaPot;

public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
}

public MainWindow(MainWindowViewModel viewModel)
    : this()
{
    DataContext = viewModel;
}

}

