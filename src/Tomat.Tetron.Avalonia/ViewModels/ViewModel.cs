using Avalonia;
using ReactiveUI;
using Tomat.Tetron.Avalonia.Services;

namespace Tomat.Tetron.Avalonia.ViewModels;

public abstract class ViewModel : ReactiveObject { }

public abstract class ConfigurableViewModel<TThis> : ViewModel where TThis : ConfigurableViewModel<TThis>, new() {
    protected static ConfigurationService ConfigurationService => AvaloniaLocator.Current.GetService<ConfigurationService>()!;

    /// <summary>
    ///     Called when this configurable view model is first instantiated,
    ///     prior to ever being loaded from configuration.
    /// </summary>
    protected virtual void Initialize() { }

    public static TThis Load() {
        var config = ConfigurationService.GetConfig<TThis>();
        if (config is not null)
            return config;

        config = new TThis();
        config.Initialize();
        ConfigurationService.RegisterConfig(config);
        return config;
    }
}
