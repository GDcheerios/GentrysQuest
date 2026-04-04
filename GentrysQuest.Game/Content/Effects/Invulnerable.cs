using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Effects;

public class Invulnerable : StatusEffect
{
    public Invulnerable(int duration = 0, int stack = 0)
        : base(duration, stack)
    {
        OnRemove += delegate { Effector.DamageModifier = 1; };
    }

    public override string Name { get; set; } = "Invulnerable";
    public override string Description { get; set; } = "Effector can't take damage";
    public override Colour4 EffectColor { get; protected set; } = Colour4.Gray;
    public override bool IsInfinite { get; set; }

    public override void Handle()
    {
        Effector.DamageModifier = 0;
    }
}
