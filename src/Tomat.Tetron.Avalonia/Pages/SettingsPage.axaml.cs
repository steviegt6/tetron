using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tomat.Tetron.Avalonia.ViewModels.Pages;

namespace Tomat.Tetron.Avalonia.Pages;

public sealed partial class SettingsPage : UserControl {
    public SettingsPage() {
        InitializeComponent();
        DataContext = SettingsPageViewModel.Load();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
