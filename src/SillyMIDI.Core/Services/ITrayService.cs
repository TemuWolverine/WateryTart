using Avalonia.Controls;

namespace SillyMIDI.Core.Services;

public interface ITrayService
{
    void CreateTrayIcon();
    void Initialize(Window mainWindow);
    void Dispose();
}
