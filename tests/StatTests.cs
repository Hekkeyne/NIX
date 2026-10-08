using Nix.Core.Modifiers;
using Nix.Core.Stats;
using Xunit;
namespace Nix.Tests;
public class StatTests
{
    [Fact]
    public void StatValues_Empty_HasDefaultValues()
    {
        var value = StatValues.Empty;
        Assert.Equal(0, value.Force);
        Assert.Equal(0, value.Signal);
        Assert.Equal(0, value.Shell);
        Assert.Equal(100f, value.MaxHp);
        Assert.Equal(200f, value.MoveSpeed);
        Assert.Equal(10f, value.BaseDamage);
    }
    [Fact]
    public void AdditiveModifier_ForAttribute_IncreasesOnlyTargetColor()
    {
        var modifier = AdditiveModifier.ForAttribute("ring_of_force", BuildColor.Force, 5);
        var result = modifier.Apply(StatValues.Empty, 0);
        Assert.Equal(5, result.Force);
        Assert.Equal(0, result.Signal); 
        Assert.Equal(0, result.Shell);
    }
    [Fact]
    public void AdditiveModifier_ForHp_DoesNotTouchColors()
    {
        var modifier = AdditiveModifier.ForHp("vitality_charm", 25f);
        var result = modifier.Apply(StatValues.Empty, 0);
        Assert.Equal(125f, result.MaxHp);
        Assert.Equal(0, result.Force);
    }
    [Fact]
    public void ModifierStack_AddThenRemove_ReturnsToBase()
    {
        var stack = new ModifierStack(StatValues.Empty);
        var modifier = AdditiveModifier.ForAttribute("ring", BuildColor.Force, 5);
        stack.Add(modifier);
        Assert.Equal(5, stack.CurrentValue.Force);
        stack.Remove(modifier);
        Assert.Equal(0, stack.CurrentValue.Force);
    }
    [Fact]
    public void StatValues_With_ReturnsNewCard_OldUnchanged()
    {
        var original = StatValues.Empty;
        var modified = original.With(BuildColor.Signal, 7);
        Assert.Equal(7, modified.Signal);
        Assert.Equal(0, original.Signal);
    }
}