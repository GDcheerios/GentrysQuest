using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Effects
{
    public class Disarm : StatusEffect
    {
        public Disarm(int duration = 1, int stack = 1)
            : base(duration, stack) =>
            OnRemove += delegate { Effector.CanAttack = true; };

        public override string Name { get; set; } = "Disarm";
        public override string Description { get; set; } = "Disarm effect";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Gray;
        public override bool IsInfinite { get; set; } = false;

        public override void Handle() => Effector.CanAttack = false;
    }
}
