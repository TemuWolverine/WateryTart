using IconPacks.Avalonia.Material;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Settings;

public interface IHaveSettings : IViewModelBase
{
    public PackIconMaterialKind Icon { get; }

    public string Description { get; }

}