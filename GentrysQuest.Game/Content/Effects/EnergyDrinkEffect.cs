using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Effects
{
    public class EnergyDrinkEffect : StatusEffect
    {
        private string sourceKey;

        public EnergyDrinkEffect()
            : base(new Second(10))
        {
            OnRemove += delegate
            {
                if (sourceKey != null)
                    Effector.StatModifiers.RemoveSource(sourceKey);

                Effector.RefreshStatModifiers();
            };
        }

        public override string Name { get; set; } = "Energy Drink";
        public override string Description { get; set; } = "Attack +30%, Speed +20%, Defense -40%";
        public override Colour4 EffectColor { get; protected set; } = Colour4.Orange;
        public override bool IsInfinite { get; set; } = false;

        public override int Duration { get; protected set; } = new Second(10);

        public override void Handle()
        {
            Active = true;
            sourceKey ??= $"effect:energy-drink:{ID}";

            // TODO: Implement drink sound effect
            Effector.StatModifiers.SetSource(sourceKey,
            [
                StatModifier.PercentOfDefault(StatType.Attack, 30 * Stack),
                StatModifier.PercentOfDefault(StatType.Speed, 20 * Stack),
                StatModifier.PercentOfDefault(StatType.Defense, -40 * Stack)
            ]);

            Effector.RefreshStatModifiers();
        }
    }
}
