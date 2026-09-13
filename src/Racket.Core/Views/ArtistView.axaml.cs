using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class ArtistView : ReactiveUserControl<ArtistViewModel>
{
    public ArtistView()
    {
        InitializeComponent();
    }
}