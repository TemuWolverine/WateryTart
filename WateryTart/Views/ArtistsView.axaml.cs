using ReactiveUI.Avalonia;
using WateryTart.ViewModels;

namespace WateryTart.Views;

public partial class ArtistsView : ReactiveUserControl<ArtistsViewModel>
{
    public ArtistsView()
    {
        InitializeComponent();
    }
}