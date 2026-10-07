using Nix.Core.Stats;

namespace Nix.Core.Modifiers;
public enum ModifierType
{
    Additive,
    Multiplicative,
    Override

}
public interface IModifier
{
    ModifierType Type { get; }
    string Id { get; }
    StatValues Apply(StatValues current, int orderHint);
}