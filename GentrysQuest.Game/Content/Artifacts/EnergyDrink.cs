using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class EnergyDrink : Artifact
    {
        public override AllowedPercentMethod AllowedPercentMethod { get; set; } = AllowedPercentMethod.OnlyPercent;
        public override string Name { get; set; } = "Energy Drink";

        public override string Description { get; protected set; } = "Very tasty and addicting. "
                                                                     + "Replaces Secondary with [type]Energy Drink[/type]. "
                                                                     + "Raises "
                                                                     + "[stat]Attack[/stat] by [unit]30%[/unit] "
                                                                     + "[stat]Speed[/stat] by [unit]20%[/unit] "
                                                                     + "and decreases "
                                                                     + "[stat]Defense[/stat] by [unit]40%[/unit] "
                                                                     + "for 10 seconds.";

        public EnergyDrink()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_energy_drink.png");
        }

        private Skill oldSkill;

        public override void OnEquip(Entity.Entity entity)
        {
            oldSkill = Holder.Secondary;
            Holder.Secondary = new Skills.EnergyDrink();
        }

        public override void OnUnequip(Entity.Entity entity) => Holder.Secondary = oldSkill;
    }
}
