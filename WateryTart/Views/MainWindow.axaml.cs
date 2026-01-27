using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
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