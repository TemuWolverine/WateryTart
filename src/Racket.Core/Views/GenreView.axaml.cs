using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core;

public partial class GenreView : ReactiveUserControl<GenreViewModel>
{
    public GenreView()
    {
        InitializeComponent();
    }
}