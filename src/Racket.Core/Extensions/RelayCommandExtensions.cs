using CommunityToolkit.Mvvm.Input;

namespace Racket.Core.Extensions;

public static class RelayCommandExtensions
{
    public static void ExecuteIfCan(this RelayCommand command, object? parameter = null)
    {
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
