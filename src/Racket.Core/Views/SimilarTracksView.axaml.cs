using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class SimilarTracksView : ReactiveUserControl<SimilarTracksViewModel>
{
    public SimilarTracksView()
    {
        InitializeComponent();
    }
}