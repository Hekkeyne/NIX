using Godot;
using GdInput = Godot.Input;
using Nix.Core.Input;
using System;
namespace Nix.Core.Input;
public sealed class DesktopInputProvider : IInputProvider
{
    private const float StickDeadZone = 0.25f;
    private readonly Func<Vector2> _pointerViewportpos;
    private bool _pendingRoll;
    private bool _pendingInteract;
    private bool _pendingSwapWeapon;
    private bool _pendingUseActive;
    private bool _pendingPause;
    public DesktopInputProvider(Func<Vector2> pointerViewportPos)
    {
        ArgumentNullException.ThrowIfNull(pointerViewportPos);
        _pointerViewportpos = pointerViewportPos;
    }
    public void OnInputEvent(InputEvent e)
    {
        if (e.IsActionPressed("roll")) _pendingRoll = true;
        if (e.IsActionPressed("interact")) _pendingInteract = true;
        if (e.IsActionPressed("swap_weapon")) _pendingSwapWeapon = true;
        if (e.IsActionPressed("use_active")) _pendingUseActive = true;
        if (e.IsActionPressed("pause")) _pendingPause = true;
    }
    public InputSnapshot Poll()
    {
        var move = GdInput.GetVector("move_left", "move_right", "move_up", "move_down");
        var aimStick = GdInput.GetVector("aim_left", "aim_right", "aim_up", "aim_down");
        aimStick = aimStick.Length() < StickDeadZone ? Vector2.Zero : aimStick.Normalized();
        var snapshot = new InputSnapshot
        {
            Move = move,
            AimStick = aimStick,
            AimFromPointer = aimStick == Vector2.Zero,
            PointerViewportPos = _pointerViewportpos(),
            RollPressed = _pendingRoll,
            InteractPressed = _pendingInteract,
            SwapWeaponPressed = _pendingSwapWeapon,
            UseActivePressed = _pendingUseActive,
            PausePressed = _pendingPause,
        };
        _pendingRoll = false;
        _pendingInteract = false;
        _pendingSwapWeapon = false;
        _pendingUseActive = false;
        _pendingPause = false;
        return snapshot;
    }
}