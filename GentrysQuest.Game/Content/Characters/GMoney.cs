using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class GMoney : Character
    {
        public GMoney()
        {
            Name = "GMoney";
            StarRating = new StarRating(5);
            Description = "Hy-plains Drifter.";

            TextureMapping = new();
            TextureMapping.Add("Idle", "characters_gmoney_idle.png");

            Stats.Health.Point = 2;
            Stats.Attack.Point = 2;

            Secondary = new FrisbeeThrow();
        }
    }
}
