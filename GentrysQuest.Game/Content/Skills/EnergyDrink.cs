using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Skills;

public class EnergyDrink : Skill
{
    public override string Name { get; protected set; } = "Energy Drink";

    public override string Description { get; protected set; } = "Very tasty and addicting. "
                                                                 + "Replaces Secondary with [type]Energy Drink[/type]. "
                                                                 + "Raises "
                                                                 + "[stat]Attack[/stat] by [unit]30%[/unit] "
                                                                 + "[stat]Speed[/stat] by [unit]20%[/unit] "
                                                                 + "and decreases "
                                                                 + "[stat]Defense[/stat] by [unit]40%[/unit] "
                                                                 + "for 10 seconds.";

    public override double Cooldown { get; protected set; } = 30;

    protected override void SkillDo()
    {
        // User.GetBase().Stats.Attack.
    }
}
