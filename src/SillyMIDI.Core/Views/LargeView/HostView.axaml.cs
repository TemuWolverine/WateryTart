using Avalonia.Controls;
using System;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views.LargeView
{
    public partial class HostView : UserControl
    {
        public HostView()
        {
            InitializeComponent();
        }

        private void HostView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            _ = vm.Connect();
            vm.Router.CurrentViewModel.Subscribe((_) =>
            {
                var sv = this.Find<ScrollViewer>("sv");
                sv?.ScrollToHome();
            });

            // App.Launcher = TopLevel.GetTopLevel(this).Launcher;
        }
    }
}