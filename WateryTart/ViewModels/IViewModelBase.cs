using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WateryTart.ViewModels;

public interface IViewModelBase : IRoutableViewModel
{
    public string Title { get; set; }
}