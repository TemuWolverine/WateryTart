using System.Threading.Tasks;

namespace SillyMIDI.Core.ViewModels
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