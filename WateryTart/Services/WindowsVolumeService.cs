

using SharpHook;
using SharpHook.Providers;

namespace WateryTart.Services;

public interface IHandleShutdown
{

}
public class WindowsVolumeService : IVolumeService, IHandleShutdown
{
    private readonly IPlayersService playerService;

    public bool IsEnabled { get; set; }

    public WindowsVolumeService(IPlayersService playerService)
    {
        UioHookProvider.Instance.KeyTypedEnabled = false;
        var hook = new EventLoopGlobalHook(SharpHook.Data.GlobalHookType.Keyboard);
        hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
        hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>
        hook.KeyReleased += OnKeyReleased;


        hook.RunAsync();
        this.playerService = playerService;
    }

    private void OnHookDisabled(object? sender, HookEventArgs e)
    {

    }

    private void OnHookEnabled(object? sender, HookEventArgs e)
    {

    }

    private void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        switch (e.Data.KeyCode)
        {
            case SharpHook.Data.KeyCode.VcVolumeUp:
                playerService.PlayerVolumeUp();
                break;
            case SharpHook.Data.KeyCode.VcVolumeDown:
                playerService.PlayerVolumeDown();
                break;
        }
    }
}
