using System;
namespace Nix.Core.Stats;

public readonly record struct StatValues
{
    public int Force { get; init; }
    public int Signal { get; init; }
    public int Shell { get; init; }
    public float MaxHp { get; init; }
    public float MoveSpeed { get; init; }
    public float BaseDamage { get; init; }
    public static StatValues Empty => new()
    {
        Force = 0,
        Signal = 0,
        Shell = 0,
        MaxHp = 0,
        MoveSpeed = 0,
        BaseDamage = 0
    };
    public int Get(BuildColor attr) => attr switch
    {
        BuildColor.Force => Force,
        BuildColor.Signal => Signal,
        BuildColor.Shell => Shell,
        _ => throw new ArgumentOutOfRangeException(nameof(attr), attr, "Неизвестный атрибут")
    };
    public StatValues With(BuildColor attr, int value) => attr switch
    {
        BuildColor.Force => this with { Force = value },
        BuildColor.Signal => this with { Signal = value },
        BuildColor.Shell => this with { Shell = value },
        _ => throw new ArgumentOutOfRangeException(nameof(attr), attr, "Неизвестный атрибут")
    };
}