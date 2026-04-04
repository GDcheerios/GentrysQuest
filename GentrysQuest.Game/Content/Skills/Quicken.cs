using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Skills
{
    public class Quicken : Skill
    {
        public override string Name { get; protected set; } = "Quicken";
        public override string Description { get; protected set; } = "increases speed by 2 and attack speed by 0.7 for 4 seconds";
        public override double Cooldown { get; protected set; } = new Second(15);

        protected override void SkillDo()
        {
            User.GetBase().AddEffect(new QuickenEffect());
        }
    }

    public class QuickenEffect : StatusEffect
    {
        private string sourceKey;

        public QuickenEffect()
            : base(new Second(4))
        {
            OnRemove += delegate
            {
                if (sourceKey != null)
                    Effector.StatModifiers.RemoveSource(sourceKey);

                Effector.RefreshStatModifiers();
            };
        }

        public override string Name { get; set; } = "Quicken";
        public override string Description { get; set; } = "";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Gray;
        public override bool IsInfinite { get; set; } = false;

        public override int Duration { get; protected set; } = new Second(4);

        public override void Handle()
        {
            Active = true;
            sourceKey ??= $"effect:quicken:{ID}";

            Effector.StatModifiers.SetSource(sourceKey,
            [
                StatModifier.Flat(StatType.Speed, 2 * Stack),
                StatModifier.Flat(StatType.AttackSpeed, 0.7 * Stack)
            ]);

            Effector.RefreshStatModifiers();
        }
    }
}
