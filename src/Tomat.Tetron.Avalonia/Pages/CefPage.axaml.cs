using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tomat.Tetron.Avalonia.ViewModels.Pages;

namespace Tomat.Tetron.Avalonia.Pages;

public sealed partial class CefPage : UserControl {
    public CefPage() {
        InitializeComponent();
        DataContext = new CefPageViewModel();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
