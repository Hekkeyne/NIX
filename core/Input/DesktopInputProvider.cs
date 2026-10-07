using Godot;
using GdInput = Godot.Input;
using Nix.Core.Input;
using System;
namespace Nix.Core.Input;
public sealed class DesktopInputProvider : IInputProvider
{
    private const float StickDeadZone = 0.25f;
    private readonly Func<Vector2> _pointerViewportpos;
    public DesktopInputProvider(Func<Vector2> pointerViewportPos)
    {
        ArgumentNullException.ThrowIfNull(pointerViewportPos);
        _pointerViewportpos = pointerViewportPos;
    }
    public InputSnapshot Poll()
    {
        var move = GdInput.GetVector("move_left", "move_right", "move_down", "move_up");
        var aimStick = GdInput.GetVector("aim_left", "aim_right", "aim_down", "aim_up");
        aimStick = aimStick.Length() < StickDeadZone ? Vector2.Zero : aimStick.Normalized();
        return new InputSnapshot
        {
            Move = move,
            AimStick = aimStick,
            AimFromPointer = aimStick == Vector2.Zero,
            PointerViewportPos = _pointerViewportpos(),
            RollPressed = GdInput.IsActionJustPressed("roll"),
            InteractPressed = GdInput.IsActionJustPressed("interact"),
            SwapWeaponPressed = GdInput.IsActionJustPressed("swap_weapon"),
            UseActivePressed = GdInput.IsActionJustPressed("use_active"),
            PausePressed = GdInput.IsActionJustPressed("pause"),
        };
    }
}