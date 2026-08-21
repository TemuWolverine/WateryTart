using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WateryTart.Core.Services;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels
{
    public partial class GenreViewModel : ViewModelBase<GenreViewModel>, ILoadableViewModel<Genre>, ILoadAsync
    {

        [Reactive] public partial Genre Genre { get; set; }
        [Reactive] public partial ObservableCollection<GenreOverview> GenreOverview { get; set; }
        public GenreViewModel(MusicAssistantClient massClient, IScreen screen, PlayersService playersService, Genre? genre = null)
            : base(client: massClient, playersService: playersService)
        {
            HostScreen = screen;
            Genre = genre ?? new Genre();
            GenreOverview = new ObservableCollection<GenreOverview>();
        }

        public GenreOverview? SelectedTab { get; set; }

        public async Task LoadAsync()
        {
            if (Genre == null || Genre.ItemId == null || Genre.Provider == null)
                return;

            IsLoading = true;
            try
            {
                var genreResponse = await _client.WithWs().GetGenreAsync(Genre.ItemId, Genre.Provider);
                if (genreResponse?.Result != null)
                {
                    Genre = genreResponse.Result;
                    Title = genreResponse.Result.Name;
                }

                var genreOverviewResponse = await _client.WithWs().GetGenreOverviewAsync(Genre.ItemId);
                if (genreOverviewResponse?.Result != null)
                {
                    GenreOverview.Clear();
                    foreach (var overview in genreOverviewResponse.Result)
                    {
                        GenreOverview.Add(overview);
                    }
                }
            }
            catch (Exception ex)
            {
                App.Logger?.LogError(ex, $"Error loading genre");
            }

            try
            {

            }
            catch (Exception ex)
            {
                App.Logger?.LogError(ex, $"Error loading genre tracks");
            }

            IsLoading = false;
        }

        public async Task SetAndLoadModel(Genre item)
        {
            Genre = item;
            await LoadAsync();
        }
    }
}