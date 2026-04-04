using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Content.Effects
{
    public class ChosenOne : StatusEffect
    {
        public override string Name { get; set; } = "Chosen One";
        public override string Description { get; set; } = "Boosts all stats by 20% per difficulty";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Gold;
        public override IconUsage Icon { get; protected set; } = FontAwesome.Solid.UserPlus;
        public override bool IsInfinite { get; set; } = true;

        private bool boosted;

        public override void Reset()
        {
            base.Reset();
            boosted = false;
        }

        public override void Handle()
        {
            if (boosted) return;

            Effector.Stats.Boost(Stack * 20);
            boosted = true;
        }
    }
}
