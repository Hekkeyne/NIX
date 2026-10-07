using System;
using System.Collections.Generic;

namespace Nix.Core.Events;

public sealed class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var key = typeof(TEvent);
        if (!_handlers.TryGetValue(key, out var list))
        {
            list = new List<Delegate>();
            _handlers[key] = list;
        }
        list.Add(handler);
        return new Subscription(this, key, handler);
    }
    public void Publish<TEvent>(TEvent evt)
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var list))
            return;
        foreach (var handler in list.ToArray())
            ((Action<TEvent>)handler)(evt);
    }
    private void Unsubscribe(Type key, Delegate handler)
    {
        if (_handlers.TryGetValue(key, out var list))
            list.Remove(handler);
    }
    private sealed class Subscription : IDisposable
    {
        private EventBus? _bus;
        private readonly Type _key;
        private readonly Delegate _handler;
        public Subscription(EventBus bus, Type key, Delegate handler)
        {
            _bus = bus;
            _key = key;
            _handler = handler;
        }
        public void Dispose()
        {
            if (_bus is null) return;
            _bus.Unsubscribe(_key, _handler);
            _bus = null;
        }
    }
}