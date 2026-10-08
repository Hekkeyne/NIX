using System;
using Godot;
using Nix.Core.Events;
using Nix.Core.Input;
using Nix.Services.Settings;
namespace Nix.Services.Input;
public sealed class InputService : IDisposable
{
    private readonly IInputProvider _desktop;
    private readonly IInputProvider _touch;
    private readonly SettingsService _settings;
    private readonly IDisposable _settingsToken;
    private readonly bool _deviceIsTouch;
    public InputSnapshot Current { get; private set; } = InputSnapshot.Empty;
    public bool IsAimAssistActive { get; private set; }
    public InputService(SettingsService settings, TouchInputState touchState,
                        EventBus bus, Func<Vector2> pointerViewportPos)
    {
        _settings = settings;
        _desktop = new DesktopInputProvider(pointerViewportPos);
        _touch = new TouchInputProvider(touchState);
        _deviceIsTouch = DisplayServer.IsTouchscreenAvailable() || OS.HasFeature("mobile");
        _settingsToken = bus.Subscribe<SettingsChangedEvent>(_ => RecalcAssist());
        RecalcAssist();
    }
    public void Poll() => Current = ActiveProvider.Poll();
    private IInputProvider ActiveProvider => _deviceIsTouch ? _touch : _desktop;
    private void RecalcAssist()
    {
        IsAimAssistActive = _settings.AimAssist switch
        {
            AimAssistMode.On => true,
            AimAssistMode.Off => false,
            _ => _deviceIsTouch,
        };
    }
    public void OnInputEvent(InputEvent e) => ActiveProvider.OnInputEvent(e);
    public void Dispose() => _settingsToken.Dispose();
}