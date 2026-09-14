using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SillyMIDI.Core.ViewModels.LargeView
{
    public class SidebarViewModel
    {
        public ObservableCollection<string> Items { get; set; }
        public SidebarViewModel()
        {
            Items = new ObservableCollection<string>
            {
                "Discover",
                "Search",
                "Browse",
                "Artists",
                "Albums",
                "Tracks",
                "Playlists",
                "Radio",
                "Podcasts",
                "Genres",
            };
        }
    }
}
