using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tomat.Tetron.Avalonia.Pages;

public sealed partial class CefPage : UserControl {
    public CefPage() {
        InitializeComponent();
        
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
