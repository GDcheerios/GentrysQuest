using GentrysQuest.Game.Entity;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Content.Characters
{
    public class Airxy : Character
    {
        public Airxy()
        {
            Name = "Airxy";
            Description = "Air Fire Wing7."
                          + "[stat]Health[/stat] is converted into[type]lives[/type].";
            StarRating = new StarRating(5);

            EffectModifier = 0;
            HealingModifier = 0;

            Stats.Health.Minimum = new Bindable<double>(5);
            Stats.Health.SetDefaultValue(1);

            Stats.Attack.Point = 4;
            Stats.Attack.Minimum = new Bindable<double>(50);

            Stats.Defense = new AirxyDefense("Defense", StatType.Defense, 0);

            OnGetHit += details =>
            {
                details.Damage = 1;
                details.IgnoreDefense = true;
            };
        }

        public override void UpdateStats()
        {
            base.UpdateStats();
            Stats.Health.SetDefaultValue(5 * Difficulty);
        }
    }

    public class AirxyDefense(string name, StatType statType, double minimumValue, bool resetsOnUpdate = true)
        : Stat(name, statType, minimumValue, resetsOnUpdate)
    {
        public override double GetDefault() => 0;
        public override double GetAdditional() => 0;
        public override double GetCurrent() => 0;
        public override double Total() => 0;
    }
}
