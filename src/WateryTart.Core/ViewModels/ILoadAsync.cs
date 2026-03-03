using System.Threading.Tasks;

namespace WateryTart.Core.ViewModels
{
    public interface ILoadableViewModel<T>
    {
        Task SetAndLoadModel(T item);
    }
    public interface ILoadAsync
    {
        Task LoadAsync();
    }
}