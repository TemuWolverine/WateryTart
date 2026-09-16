using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class ArtistView : ReactiveUserControl<ArtistViewModel>
{
    public ArtistView()
    {
        InitializeComponent();
    }
}