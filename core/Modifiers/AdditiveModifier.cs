using Nix.Core.Stats;
namespace Nix.Core.Modifiers;
public sealed class AdditiveModifier : IModifier
{
    public ModifierType Type => ModifierType.Additive;
    public string Id { get; }
    private readonly BuildColor? _color;
    private readonly int _colorDelta;
    private readonly float _hpDelta;
    private readonly float _moveSpeedDelta;
    private readonly float _baseDamageDelta;
    private AdditiveModifier(string id, BuildColor? color, int colorDelta,
                             float hpDelta, float moveSpeedDelta, float baseDamageDelta)
    {
        Id = id;
        _color = color;
        _colorDelta = colorDelta;
        _hpDelta = hpDelta;
        _moveSpeedDelta = moveSpeedDelta;
        _baseDamageDelta = baseDamageDelta;
    }
    public static AdditiveModifier ForAttribute(string id, BuildColor color, int delta)
        => new(id, color, delta, 0f, 0f, 0f);
    public static AdditiveModifier ForHp(string id, float delta)
        => new(id, null, 0, delta, 0f, 0f);
    public static AdditiveModifier ForMoveSpeed(string id, float delta)
        => new(id, null, 0, 0f, delta, 0f);
    public static AdditiveModifier ForBaseDamage(string id, float delta)
        => new(id, null, 0, 0f, 0f, delta);
    public StatValues Apply(StatValues current, int orderHint)
    {
        if (_color.HasValue)
        {
            return current.With(_color.Value, current.Get(_color.Value) + _colorDelta);
        }
        return current with
        {
            MaxHp = current.MaxHp + _hpDelta,
            MoveSpeed = current.MoveSpeed + _moveSpeedDelta,
            BaseDamage = current.BaseDamage + _baseDamageDelta
        };
    }
}