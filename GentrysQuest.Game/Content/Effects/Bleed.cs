using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Content.Effects
{
    public class Bleed(int duration = 1, int stack = 1) : StatusEffect(duration, stack)
    {
        public override string Name { get; set; } = "Bleed";

        public override string Description { get; set; } =
            "Lose 3 + Level per stack health every 0.5 seconds";

        public override Colour4 EffectColor { get; protected set; } = Colour4.DarkRed;
        public override IconUsage Icon { get; protected set; } = FontAwesome.Solid.Splotch;
        public override bool IsInfinite { get; set; }
        public override double Interval { get; protected set; } = new Second(0.5);

        public override void Handle()
        {
            if (ElapsedTime() > Interval * CurrentStep)
            {
                CurrentStep++;
                DamageDetails damageDetails = new DamageDetails
                {
                    Receiver = Effector,
                    StatusEffect = this,
                    Damage = (int)((3 + Effector.Experience.CurrentLevel() * Stack) * Effector.EffectModifier),
                };
                Effector.Damage(damageDetails);
                Effector.DisplayHealthEvent($"{damageDetails.Damage}", ColourInfo.GradientVertical(EffectColor, Colour4.Black));
            }
        }
    }
}
