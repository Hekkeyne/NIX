using Godot;
using Nix.Core.Input;

namespace Nix.Services.Input;
public sealed class TouchInputProvider : IInputProvider
{
    private const float StickDeadzone = 0.2f;
    private readonly TouchInputState _state;

    public TouchInputProvider(TouchInputState state) => _state = state;

    public InputSnapshot Poll()
    {
        var snapshot = new InputSnapshot
        {
            Move = ApplyDeadzone(_state.Move),
            AimStick = ApplyDeadzone(_state.AimStick),
            AimFromPointer = false,
            RollPressed = _state.RollPressed,
            InteractPressed = _state.InteractPressed,
            SwapWeaponPressed = _state.SwapWeaponPressed,
            UseActivePressed = _state.UseActivePressed,
            PausePressed = false,
        };
        _state.ClearFrame();
        return snapshot;
    }
    public void OnInputEvent(InputEvent e) { }
    private static Vector2 ApplyDeadzone(Vector2 v)
        => v.Length() < StickDeadzone ? Vector2.Zero : v.Normalized();
}