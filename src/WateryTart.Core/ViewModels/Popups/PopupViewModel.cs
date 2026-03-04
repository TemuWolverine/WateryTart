using ReactiveUI;

namespace WateryTart.Core.ViewModels.Popups;
public class PopupViewModel(string message, string? title = null) : ReactiveObject, IPopupViewModel
{
    public string Message { get; } = message;
    public string? Title { get; } = title;
}