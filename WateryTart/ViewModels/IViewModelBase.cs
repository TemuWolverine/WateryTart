using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WateryTart.ViewModels;

public interface IViewModelBase : IRoutableViewModel
{
    [Reactive] public string Title { get; set; }
}