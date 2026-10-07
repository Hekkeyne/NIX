using Nix.Core.Events;
namespace Nix.Services.Settings;
public enum AimAssistMode { Auto,On,Off}
public sealed class SettingsService
{
    private readonly EventBus _bus;
    public AimAssistMode AimAssist { get;private set;  }=AimAssistMode.Auto;
    public SettingsService(EventBus bus)
    {
        _bus = bus;
    }
    public void SetAimAssist(AimAssistMode mode)
    {
        if (AimAssist == mode) return;
        AimAssist = mode;
        _bus.Publish(new SettingsChangedEvent());
    }
}