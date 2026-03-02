using System.Threading.Tasks;

namespace WateryTart.Core.ViewModels
{
    public interface INeedsLoadingViewModel
    {
        Task LoadAsync();
    }
}