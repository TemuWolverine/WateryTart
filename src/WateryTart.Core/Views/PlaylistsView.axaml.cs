using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core.Views;

public partial class PlaylistsView : ReactiveUserControl<PlaylistsViewModel>
{
    public PlaylistsView()
    {
        InitializeComponent();
    }
}