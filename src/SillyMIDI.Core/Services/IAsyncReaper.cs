using System.Threading.Tasks;

namespace SillyMIDI.Core.Services;

public interface IAsyncReaper : IReaper
{
    Task ReapAsync();
}