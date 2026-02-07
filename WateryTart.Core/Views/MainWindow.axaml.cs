using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using WateryTart.Core.Services;
using WateryTart.Core.Settings;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    private ISettings? _settings;
    private ITrayService? _trayService;
    public MainWindow()
    {
        this.WhenActivated(disposables =>
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null)
                return;

            _ = vm.Connect();
            vm.Router.CurrentViewModel.Subscribe((_) =>
            {
                var sv = this.Find<ScrollViewer>("sv");
                sv?.ScrollToHome();
            });

            _settings = App.Container.GetRequiredService<ISettings>();
            if (_settings.WindowWidth != 0)
            {
                Width = _settings.WindowWidth;
                Height = _settings.WindowHeight;
                Position = new Avalonia.PixelPoint((int)_settings.WindowPosX, (int)_settings.WindowPosY);
            }

            Resized += MainWindow_Resized;
            PositionChanged += MainWindow_PositionChanged;

            // Initialize tray service
            try
            {
                _trayService = new TrayService();
                _trayService.Initialize(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Tray service initialization failed: {ex.Message}");
            }
        });

        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnClosed(EventArgs e)
    {
        _trayService?.Dispose();
        base.OnClosed(e);
    }

    private void MainWindow_PositionChanged(object? sender, PixelPointEventArgs e)
    {
        _settings.WindowPosX = e.Point.X;
        _settings.WindowPosY = e.Point.Y;
    }

    private void MainWindow_Resized(object? sender, WindowResizedEventArgs e)
    {
        _settings.WindowWidth = e.ClientSize.Width;
        _settings.WindowHeight = e.ClientSize.Height;
    }
}