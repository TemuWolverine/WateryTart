using System.Threading.Tasks;

namespace WateryTart.Services;

public interface IAsyncReaper : IReaper
{
    Task ReapAsync();
}