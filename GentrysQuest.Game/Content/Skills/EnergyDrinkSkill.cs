using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Skills
{
    public class EnergyDrinkSkill : Skill
    {
        public override string Name { get; protected set; } = "Energy Drink";

        public override string Description { get; protected set; } = "Raises "
                                                                     + "[stat]Attack[/stat] by [unit]30%[/unit] "
                                                                     + "[stat]Speed[/stat] by [unit]20%[/unit] "
                                                                     + "and decreases "
                                                                     + "[stat]Defense[/stat] by [unit]40%[/unit] "
                                                                     + "for 10 seconds.";

        public override double Cooldown { get; protected set; } = 30;

        protected override void SkillDo() => User.GetBase().AddEffect(new EnergyDrinkEffect());
    }
}
