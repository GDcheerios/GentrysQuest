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

            Stats.Health.point = 2;
            Stats.Attack.point = 2;

            Secondary = new FrisbeeThrow();
        }
    }
}
