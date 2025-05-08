using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class StarterCharacter : Character
    {
        public StarterCharacter()
        {
            Name = "Starter Character";
            StarRating = new StarRating(1);
            Description = "The guy";

            Secondary = new Heal();
            Utility = new Quicken();
        }
    }
}
