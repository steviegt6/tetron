using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tomat.Tetron.Avalonia.ViewModels.Pages;

namespace Tomat.Tetron.Avalonia.Pages;

public sealed partial class HomePage : UserControl {
    public HomePage() {
        InitializeComponent();
        DataContext = HomePageViewModel.Load();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
