using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Content.Effects
{
    public class Stun : StatusEffect
    {
        public Stun(int duration = 1, int stack = 1)
            : base(duration, stack)
        {
            OnRemove += delegate
            {
                Effector.CanAttack = true;
                Effector.CanMove = true;
            };
        }

        public override string Name { get; set; } = "Stun";
        public override string Description { get; set; } = "Stuns enemies";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Gray;
        public override IconUsage Icon { get; protected set; } = FontAwesome.Solid.Spinner;
        public override bool IsInfinite { get; set; } = false;

        public override void Handle()
        {
            Effector.Stats.Speed.
        }
    }
}
