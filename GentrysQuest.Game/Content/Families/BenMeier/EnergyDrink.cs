using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Families.BenMeier
{
    public class EnergyDrink : Artifact
    {
        public override AllowedPercentMethod AllowedPercentMethod { get; set; } = AllowedPercentMethod.OnlyPercent;
        public override string Name { get; set; } = "Energy Drink";
        public override string Description { get; protected set; } = "Very tasty and addicting. ";
        public override Family family { get; protected set; } = new BenMeierFamily();

        public EnergyDrink()
        {
            TextureMapping.Add("Icon", "artifacts_energy_drink.png");
        }
    }
}
