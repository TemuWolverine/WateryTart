using System.Threading.Tasks;

namespace Racket.Core.Services;

public interface IAsyncReaper : IReaper
{
    Task ReapAsync();
}