using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Beutl.Configuration;
using Beutl.Media;
using FluentAvalonia.Core;

namespace Tomat.Tetron.Avalonia;

public sealed partial class App : Application {
    private MainViewModel mainViewModel;
    
    public override void Initialize() {
        FAUISettings.SetAnimationsEnabledAtAppLevel(true);

        var type = typeof(Colors);
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Static);
        var colors = props.Select(x => x.GetValue(null)).OfType<Color>().ToArray();

        var config = GlobalConfiguration.Instance;
        config.Restore(GlobalConfiguration.DefaultFilePath);

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            // desktop.MainWindow = new MainWindow();

            desktop.Startup += (_, e) => FrameworkInitialized?.Invoke(this, e);
            desktop.Exit += (_, e) => FrameworkShutdown?.Invoke(this, e);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
