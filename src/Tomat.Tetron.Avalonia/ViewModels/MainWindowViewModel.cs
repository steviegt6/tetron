using Newtonsoft.Json;

namespace Tomat.Tetron.Avalonia.ViewModels;

public sealed class MainWindowViewModel : ConfigurableViewModel<MainWindowViewModel> {
    [JsonProperty("width")]
    public double Width { get; set; } = 800;

    [JsonProperty("height")]
    public double Height { get; set; } = 400;

    [JsonProperty("fullscreen")]
    public bool Fullscreen { get; set; }
}
