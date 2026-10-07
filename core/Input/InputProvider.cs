namespace Nix.Core.Input;
public interface IInputProvider
{
    InputSnapshot Poll();
}