using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ComAwaPot.Data;
using ComAwaPot.Services;
using ComAwaPot.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ComAwaPot;

public partial class App : Application
{
public IServiceProvider Services { get; }

public App()
{
    var services = new ServiceCollection();

    services.AddSingleton<DbContextOptions<AppDbContext>>(_ =>
        new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=ComAwaPot.db")
            .Options);

    services.AddTransient<PersonaService>();

    services.AddTransient<MainWindowViewModel>();
    services.AddTransient<PersonasViewModel>();

    Services = services.BuildServiceProvider();
}

public override void Initialize()
{
    AvaloniaXamlLoader.Load(this);
}

public override void OnFrameworkInitializationCompleted()
{
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();

        desktop.MainWindow = new MainWindow(mainWindowViewModel);
    }

    base.OnFrameworkInitializationCompleted();
}

}

