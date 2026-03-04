using ReactiveUI;

namespace WateryTart.Core
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor<TViewModel>? ResolveView<TViewModel>(string? contract = null) where TViewModel : class
        {
            
            throw new System.NotImplementedException();
        }

        public IViewFor? ResolveView(object? instance, string? contract = null)
        {
            if (ViewLocator._viewFactories.TryGetValue(instance.GetType(), out var factory))
            {
                return (IViewFor)factory();
            }

            throw new System.NotImplementedException();

        }
    }
}