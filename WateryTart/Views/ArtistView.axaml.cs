using ReactiveUI.Avalonia;
using WateryTart.ViewModels;

namespace WateryTart.Views;

public partial class ArtistView : ReactiveUserControl<ArtistViewModel>
{
    public ArtistView()
    {
        InitializeComponent();
    }
}