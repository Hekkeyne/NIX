using System.Collections.Generic;
using Godot.NativeInterop;
using Nix.Core.Modifiers;
using Nix.Core.Stats;
namespace Nix.Core.Entities;
public sealed class StatBlock
{
    private readonly ModifierStack _stack;
    public StatValues BaseValue => _stack.BaseValue;
    public StatValues CurrentValue => _stack.CurrentValue;
    public StatBlock(StatValues baseValue)
    {
        _stack = new ModifierStack(baseValue);
    }
    public void AddModifier(IModifier modifier) => _stack.Add(modifier);
    public void RemoveModifier(IModifier modifier) => _stack.Remove(modifier);
    public IReadOnlyList<IModifier> GetModifiers() => _stack.GetModifiers();
}