using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;
public partial class LibraryView : ReactiveUserControl<LibraryViewModel>
{
    public LibraryView()
    {
        InitializeComponent();
    }
}
