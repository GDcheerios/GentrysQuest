using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Content.Effects
{
    public class Stall : StatusEffect
    {
        public Stall(int duration = 1, int stack = 1)
            : base(duration, stack)
        {
            OnRemove += delegate
            {
                Effector.CanMove = true;
            };
        }

        public override string Name { get; set; } = "Stall";
        public override string Description { get; set; } = "Entity can't move";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Gray;
        public override IconUsage Icon { get; protected set; } = FontAwesome.Solid.UserClock;
        public override bool IsInfinite { get; set; } = false;
        public override bool CanStack { get; protected set; } = false;

        public override void Handle()
        {
            Effector.CanMove = false;
        }
    }
}
