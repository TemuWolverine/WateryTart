using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core;

public partial class GenreView : ReactiveUserControl<GenreViewModel>
{
    public GenreView()
    {
        InitializeComponent();
    }
}