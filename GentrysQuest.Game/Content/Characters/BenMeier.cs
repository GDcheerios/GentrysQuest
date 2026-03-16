using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class BenMeier : Character
    {
        public BenMeier()
        {
            StarRating = new StarRating(5);
            Name = "Ben Meier";
            Description = "Probably in the bathroom right now...";

            TextureMapping = new();
            TextureMapping.Add("Idle", "characters_ben_meier_idle.png");
            TextureMapping.Add("Icon", "characters_ben_meier_idle.png");

            Secondary = new EnergyDrinkSkill();

            Stats.Attack.Point = 3;
            Stats.AttackSpeed.Point = 1;
        }
    }
}
