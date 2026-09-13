using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class SearchView : ReactiveUserControl<SearchViewModel>
{
    public SearchView()
    {
        InitializeComponent();
    }
}