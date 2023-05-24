using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Beutl;
using Beutl.Configuration;
using Beutl.Media;
using FluentAvalonia.Core;
using FluentAvalonia.Styling;

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
        var view = config.ViewConfig;
        CultureInfo.CurrentUICulture = view.UICulture;

        AvaloniaXamlLoader.Load(this);
        Resources["PaletteColors"] = colors;

        view.GetObservable(ViewConfig.ThemeProperty)
            .Subscribe(x => {
                Dispatcher.UIThread.InvokeAsync(() => {
                    FluentAvaloniaTheme thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>()!;

                    switch (x) {
                        case ViewConfig.ViewTheme.Light:
                            thm.RequestedTheme = FluentAvaloniaTheme.LightModeString;
                            break;

                        case ViewConfig.ViewTheme.Dark:
                            thm.RequestedTheme = FluentAvaloniaTheme.DarkModeString;
                            break;

                        case ViewConfig.ViewTheme.HighContrast:
                            thm.RequestedTheme = FluentAvaloniaTheme.HighContrastModeString;
                            break;

                        case ViewConfig.ViewTheme.System:
                            thm.PreferSystemTheme = true;
                            thm.InvalidateThemingFromSystemThemeChanged();
                            break;
                    }
                });
            });
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
