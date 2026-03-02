using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WateryTart.MusicAssistant.Models.Enums;

namespace WateryTart.Core.ViewModels;

public interface ILoadMoreListViewModel
{
    int CurrentOffset { get; }
    RelayCommand GoToItem { get; }
    bool HasMoreItems { get; }
    bool IsLoading { get; set; }
    ObservableCollection<IViewModelBase> Items { get; }

    IEnumerable<OrderBy> SortingOptions { get; }
    OrderBy SelectedSortingOption { get; set;}
    ICommand LoadMoreCommand { get; }
    IViewModelBase? SelectedItem { get; set; }

    bool UseWrapPanel { get; }

    Task LoadAsync();
}
