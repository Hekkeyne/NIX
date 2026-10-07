using System;
using System.Collections.Generic;
using System.Linq;
using Nix.Core.Stats;
namespace Nix.Core.Modifiers;

public sealed class ModifierStack
{
    private readonly List<IModifier> _modifiers = new();
    public StatValues BaseValue { get; private set; }
    public StatValues CurrentValue { get; private set; }
    public ModifierStack(StatValues baseValue)
    {
        BaseValue = baseValue;
        CurrentValue = baseValue;
    }
    public void Add(IModifier modifier)
    {
        _modifiers.Add(modifier);
        Recalculate();
    }
    public void Remove(IModifier modifier)
    {
        _modifiers.Remove(modifier);
        Recalculate();
    }
    private void Recalculate()
    {
        var result = BaseValue;
        var sorted = _modifiers
            .Select((m, idx) => (Modifier: m, Index: idx))
            .OrderBy(x => x.Modifier.Type)
            .ThenBy(x => x.Index)
            .ToList();
        for (int i = 0; i < sorted.Count; i++)
            result = sorted[i].Modifier.Apply(result, i);
        CurrentValue = result;
    }
    public IReadOnlyList<IModifier> GetModifiers() => _modifiers.AsReadOnly();
}