using ReactiveUI;
using System.Reactive;
using System.Windows.Input;


namespace WateryTart.Core.ViewModels;

public class LibraryItem : ReactiveObject
{
    public string Title { get; set; } = string.Empty;
    public ICommand? ClickedCommand { get; set; }
    public string LowerTitle => Title.ToLowerInvariant();

    public int Count
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
