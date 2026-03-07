using Avalonia.Media;
using System.Windows.Input;

namespace WateryTart.Core.ViewModels;

public class HeroSearchModel
{
    public SolidColorBrush? BackgroundColor { get; set; }
    public ICommand? Command { get; set; }
    public string? Icon { get; set; }
    public string? Subtitle { get; set; }
    public string? Title { get; set; }
}
