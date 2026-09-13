using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views
{
    public partial class LoggerSettingsView : ReactiveUserControl<LoggerSettingsViewModel>
    {
        public LoggerSettingsView()
        {
            InitializeComponent();
        }
    }
}
