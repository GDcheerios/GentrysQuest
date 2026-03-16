using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class EnergyDrink : Artifact
    {
        public override AllowedPercentMethod AllowedPercentMethod { get; set; } = AllowedPercentMethod.OnlyPercent;
        public override string Name { get; set; } = "Energy Drink";

        public override string Description { get; protected set; } = "Very tasty and addicting. "
                                                                     + "[condition]When Secondary is used[/condition] "
                                                                     + "Raise "
                                                                     + "[stat]Attack[/stat] by [unit]30%[/unit] "
                                                                     + "[stat]Speed[/stat] by [unit]20%[/unit] "
                                                                     + "and decrease "
                                                                     + "[stat]Defense[/stat] by [unit]40%[/unit] "
                                                                     + "for 10 seconds. "
                                                                     + "[details]Has a 30 second cooldown.[/details]";

        public EnergyDrink()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_energy_drink.png");
        }

        private double lastUse;

        public override void OnEquip(Entity.Entity entity)
        {
            if (Holder.Secondary != null) Holder.Secondary.OnAct += attemptUse;
        }

        public override void OnUnequip(Entity.Entity entity)
        {
            if (Holder.Secondary != null) Holder.Secondary.OnAct -= attemptUse;
        }

        private void attemptUse()
        {
            if (!(GameClock.CurrentTime - lastUse < 30))
            {
                lastUse = GameClock.CurrentTime;
                Holder.AddEffect(new EnergyDrinkEffect());
            }
        }
    }
}
