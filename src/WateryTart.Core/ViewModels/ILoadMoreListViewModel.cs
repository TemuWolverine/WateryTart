using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WateryTart.Core.ViewModels;

public interface ILoadMoreListViewModel
{
    int CurrentOffset { get; }
    RelayCommand GoToItem { get; }
    bool HasMoreItems { get; }
    bool IsLoading { get; set; }
    ObservableCollection<IViewModelBase> Items { get; }
    ICommand LoadMoreCommand { get; }
    IViewModelBase? SelectedItem { get; set; }

    bool UseWrapPanel { get; }

    Task LoadAsync();
}
