using System;

namespace Tomat.Tetron.Avalonia;

public sealed partial class App {
    public static event EventHandler? FrameworkInitialized;

    public static event EventHandler? FrameworkShutdown;
}
