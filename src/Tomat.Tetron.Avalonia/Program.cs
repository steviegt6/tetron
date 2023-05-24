using Avalonia;
using System;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Windowing;

namespace Tomat.Tetron.Avalonia;

internal static partial class Program {
    [STAThread]
    internal static void Main(string[] args) {
        InitCef(args);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() {
        return AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseReactiveUI()
                         .LogToTrace()
                         .UseFAWindowing()
                         .With(new Win32PlatformOptions {
                             UseWindowsUIComposition = true,
                             CompositionBackdropCornerRadius = 8f,
                         });
    }
}
