using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class SimilarTracksView : ReactiveUserControl<SimilarTracksViewModel>
{
    public SimilarTracksView()
    {
        InitializeComponent();
    }
}