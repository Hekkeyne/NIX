using Godot;
using Nix.Core.Events;
using Nix.Services.Input;
using Nix.Services.Settings;
namespace Nix.Services.Bootstrap;
public partial class GameBootstrap : Node
{
    public static GameBootstrap Instance { get; private set; } = null!;
    public ServiceRegistry Services { get; private set; } = null!;

    public override void _EnterTree()
    {
        Instance = this;
        Services = new ServiceRegistry();
        var eventBus = new EventBus();
        Services.Register(eventBus);
        Services.Register(new SettingsService(eventBus));
        var touchState = new TouchInputState();
        Services.Register(touchState);
        Services.Register(new InputService(
            Services.Get<SettingsService>(),
            touchState,
            eventBus,
            pointerViewportPos: () => GetViewport().GetMousePosition()));
    }
    public override void _PhysicsProcess(double delta)
        => Services.Get<InputService>().Poll();
    public override void _UnhandledInput(InputEvent @event)
        => Services.Get<InputService>().OnInputEvent(@event);
    public override void _ExitTree()
    {
        Services.Get<InputService>().Dispose();
        if (Instance == this)
            Instance = null!;
    }
}
public static class NodeServicesExtensions
{
    public static ServiceRegistry Services(this Node node) => GameBootstrap.Instance.Services;
    public static T Svc<T>(this Node node) where T : class =>
        GameBootstrap.Instance.Services.Get<T>();
}