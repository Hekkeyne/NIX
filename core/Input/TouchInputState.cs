using Godot;
using Nix.Core.Input;
namespace Nix.Services.Input;
public sealed class TouchInputState
{
    public Vector2 Move { get; set; }
    public Vector2 AimStick { get; set; }
    public bool RollPressed { get; set; }
    public bool InteractPressed { get; set; }
    public bool SwapWeaponPressed { get; set; }
    public bool UseActivePressed { get; set; }
    public bool TouchUiActive { get; set; }
    public void ClearFrame() => RollPressed = InteractPressed = SwapWeaponPressed = UseActivePressed = false;
}