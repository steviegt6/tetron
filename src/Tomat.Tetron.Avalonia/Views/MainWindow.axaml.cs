using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Tomat.Tetron.Avalonia.Pages;
using Tomat.Tetron.Avalonia.ViewModels;

namespace Tomat.Tetron.Avalonia.Views;

public sealed partial class MainWindow : AppWindow {
    public MainWindow() {
        InitializeComponent();
        var vm = (MainWindowViewModel) (DataContext = MainWindowViewModel.Load());

        if (vm.Fullscreen)
            WindowState = WindowState.Maximized;
        Width = vm.Width;
        Height = vm.Height;

        Frame.Navigate(typeof(HomePage));
    }

    protected override void OnResized(WindowResizedEventArgs e) {
        base.OnResized(e);

        if (DataContext is not MainWindowViewModel vm)
            return;

        // Only save the size if the window is not maximized.
        if (WindowState == WindowState.Maximized) {
            vm.Fullscreen = true;
        }
        else {
            // If we were just in fullscreen then don't save the width and
            // height yet, as it preserves the fullscreen size. We change the
            // client size immediately after in a different event handler, so
            // this is fine.
            if (!vm.Fullscreen) {
                vm.Width = e.ClientSize.Width;
                vm.Height = e.ClientSize.Height;
            }

            vm.Fullscreen = false;
        }
    }

    protected override void HandleWindowStateChanged(WindowState state) {
        base.HandleWindowStateChanged(state);

        if (DataContext is not MainWindowViewModel vm)
            return;

        if (state != WindowState.Normal)
            return;

        Width = vm.Width;
        Height = vm.Height;
    }

    private void HandleNavigationViewSelection(object? sender, NavigationViewSelectionChangedEventArgs e) {
        if (e.SelectedItem is not NavigationViewItem navItem)
            return;

        if (navItem.Name == "SettingsItem")
            Frame.Navigate(typeof(SettingsPage));

        switch (navItem.Tag) {
            case nameof(HomePage):
                Frame.Navigate(typeof(HomePage));
                break;

            case nameof(CefPage):
                Frame.Navigate(typeof(CefPage));
                break;
        }
    }
}
