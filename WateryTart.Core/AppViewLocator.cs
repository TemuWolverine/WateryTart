using ReactiveUI;

namespace WateryTart.Core
{
    public class AppViewLocator : IViewLocator
    {
#pragma warning disable IL2046
#pragma warning disable IL3051
        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
#pragma warning restore IL3051
#pragma warning restore IL2046
        {
            if (viewModel != null)
                if (ViewLocator._viewFactories.TryGetValue(viewModel.GetType(), out var factory))
                {
                    return (IViewFor)factory();
                }

            return null;
        }
    }
}