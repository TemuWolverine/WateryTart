using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUI.Avalonia;
using WateryTart.ViewModels;

namespace WateryTart;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables =>
        {
            var vm = DataContext as MainWindowViewModel;
            vm.Connect();
            vm.Router.CurrentViewModel.Subscribe((_) =>
            {
                var sv = this.Find<ScrollViewer>("sv");
                if (sv != null)
                    sv.ScrollToHome();
            });

        });
        AvaloniaXamlLoader.Load(this);
    }
}