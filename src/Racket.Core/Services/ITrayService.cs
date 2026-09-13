using Avalonia.Controls;

namespace Racket.Core.Services;

public interface ITrayService
{
    void CreateTrayIcon();
    void Initialize(Window mainWindow);
    void Dispose();
}
