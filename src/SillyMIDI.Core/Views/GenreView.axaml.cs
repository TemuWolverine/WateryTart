using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core;

public partial class GenreView : ReactiveUserControl<GenreViewModel>
{
    public GenreView()
    {
        InitializeComponent();
    }
}