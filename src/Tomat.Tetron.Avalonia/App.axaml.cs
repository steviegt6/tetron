using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Tomat.Tetron.Avalonia.Services;
using Tomat.Tetron.Avalonia.ViewModels;
using Tomat.Tetron.Avalonia.Views;

namespace Tomat.Tetron.Avalonia;

public sealed partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new MainWindow {
                // DataContext = new MainWindowViewModel(),
            };

            desktop.ShutdownRequested += (_, _) => {
                AvaloniaLocator.Current.GetService<ConfigurationService>()!.SaveConfigs();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public override void RegisterServices() {
        base.RegisterServices();

        AvaloniaLocator.CurrentMutable.Bind<ConfigurationService>().ToConstant(new ConfigurationService());
    }
}
