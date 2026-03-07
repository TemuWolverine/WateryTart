using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Windows.Input;

namespace WateryTart.Core.ViewModels;

public partial class HeroSearchModel : ReactiveObject
{
    public SolidColorBrush? BackgroundColor { get; set; }
    public ICommand? Command { get; set; }
    [Reactive] public partial string? Icon { get; set; }
    public string? Subtitle { get; set; }
    public string? Title { get; set; }
}
