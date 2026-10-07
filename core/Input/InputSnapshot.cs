using Godot;
namespace Nix.Core.Input;
public readonly record struct InputSnapshot
{
    public Vector2 Move { get; init; }
    public Vector2 AimStick { get; init; }
    public bool AimFromPointer { get; init; }
    public Vector2 PointerViewportPos { get; init;  }
    public bool RollPressed { get; init; }
    public bool InteractPressed { get; init; }
    public bool SwapWeaponPressed { get; init; }
    public bool UseActivePressed { get; init; }
    public bool PausePressed { get; init; }
    public static InputSnapshot Empty => new();

}