using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class AlbumView : ReactiveUserControl<AlbumViewModel>
{
    public AlbumView()
    {
        InitializeComponent();
    }
}