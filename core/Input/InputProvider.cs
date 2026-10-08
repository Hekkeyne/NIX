using Godot;

namespace Nix.Core.Input;
public interface IInputProvider
{
    InputSnapshot Poll();
    void OnInputEvent(InputEvent e);
}