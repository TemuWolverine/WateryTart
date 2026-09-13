using IconPacks.Avalonia.Material;
using Racket.Core.ViewModels;

namespace Racket.Core.Settings;

public interface IHaveSettings : IViewModelBase
{
    public PackIconMaterialKind Icon { get; }

    public string Description { get; }

}