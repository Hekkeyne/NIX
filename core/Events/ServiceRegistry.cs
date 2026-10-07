using System;
using System.Collections.Generic;
namespace Nix.Services;
public sealed class ServiceRegistry
{
    private readonly Dictionary<Type, object> _services = new();
    public void Register<TService>(TService service) where TService : class
    {
        var key = typeof(TService);
        if (_services.ContainsKey(key))
            throw new InvalidOperationException($"Сервис {key.Name} уже зареган(((");
        _services[key] = service;
    }
    public TService Get<TService>() where TService : class
    {
        if (_services.TryGetValue(typeof(TService), out var service))
            return (TService)service;
        throw new InvalidOperationException($"Сервис {typeof(TService).Name} не зареган, чек GameBootstrap");
    }
    public bool TryGet<TService>(out TService service) where TService : class
    {
        var found = _services.TryGetValue(typeof(TService), out var obj);
        service = found ? (TService)obj : null!;
        return found;
    }

}