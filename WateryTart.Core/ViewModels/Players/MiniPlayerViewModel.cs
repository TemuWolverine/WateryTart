using System.Diagnostics;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Reactive;
using WateryTart.Core.Services;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.ViewModels.Players
{
    public partial class MiniPlayerViewModel : ReactiveObject, IViewModelBase
    {
        public string? UrlPathSegment { get; } = "MiniPlayerViewModel";
        public IScreen HostScreen { get; }
        [Reactive] public partial string Title { get; set; }
        public ReactiveCommand<Unit, Unit> PlayNextCommand { get; }
        public ReactiveCommand<Unit, Unit> PlayerPlayPauseCommand { get; }
        public ReactiveCommand<Unit, Unit> PlayerPreviousCommand { get; }
        public bool ShowMiniPlayer => false;
        public bool ShowNavigation => false;
        public ReactiveCommand<Unit, Unit> ClickedCommand { get; }

        [Reactive] public partial IPlayersService PlayersService { get; set; }

        public IScreen Screen { get; }

        

        public MiniPlayerViewModel(IPlayersService playersService, IScreen screen)
        {
            PlayersService = playersService;
            Screen = screen;
            PlayNextCommand = ReactiveCommand.Create<Unit>(_ => PlayersService.PlayerNext());
            PlayerPlayPauseCommand = ReactiveCommand.Create<Unit>(_ => PlayersService.PlayerPlayPause());
            PlayerPreviousCommand = ReactiveCommand.Create<Unit>(_ => PlayersService.PlayerPrevious());

            ClickedCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                var vm = App.Container.GetRequiredService<BigPlayerViewModel>();
                Screen.Router.Navigate.Execute(vm);
            }
            );


        }
    }
}


