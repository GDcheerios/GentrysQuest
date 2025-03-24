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

            TextureMapping.Add("Idle", "characters_ben_meier_idle.png");
            TextureMapping.Add("Icon", "characters_ben_meier_idle.png");

            Stats.Attack.Point = 3;
            Stats.AttackSpeed.Point = 1;
        }
    }
}
