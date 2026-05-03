using System;
using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class Airxy : Character
    {
        public Airxy()
        {
            Name = "Airxy";
            Description = "Air Fire Wing7."
                          + "[stat]Health[/stat] is converted into[type]lives[/type]."
                          + "[stat]Attack[/stat] scales more.";
            StarRating = new StarRating(5);

            EffectModifier = 0;
            HealingModifier = 0;

            Secondary = new CursedSpeech();

            Stats.Health.SetDefaultValue(32432);

            Stats.Attack.Point = 4;
            Stats.Attack.SetDefaultValue(50);

            Stats.Defense = new AirxyDefense("Defense", StatType.Defense);

            OnGetHit += details =>
            {
                details.Damage = 1;
                details.IgnoreDefense = true;
            };
        }

        public override void UpdateStats()
        {
            double missingHealth = Math.Max(0, Stats.Health.Total() - Stats.Health.Current.Value);

            base.UpdateStats();

            foreach (StatModifier modifier in StatModifiers.ForStat(StatType.Health))
            {
                modifier.Operation = StatModifierOperation.Flat;
                modifier.Value = 1;
            }

            RebuildStatAdditionalValues();

            Stats.Health.SetDefaultValue(5 + (5 * Difficulty));
            Stats.Health.Current.Value = Math.Clamp(Stats.Health.Total() - missingHealth, 0, Stats.Health.Total());
        }
    }

    public class AirxyDefense(string name, StatType statType)
        : Stat(name, statType)
    {
        public override double GetDefault() => 0;
        public override double GetAdditional() => 0;
        public override double GetCurrent() => 0;
        public override double Total() => 0;
    }
}
