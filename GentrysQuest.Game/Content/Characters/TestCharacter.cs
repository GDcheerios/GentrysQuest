using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class TestCharacter : Character
    {
        public override string Name { get; set; } = "Test Character";
        public override string Description { get; protected set; } = "Just the character used for testing and stuff...";

        public TestCharacter(int starRating)
        {
            StarRating = new StarRating(starRating);
            TextureMapping = new();

            Secondary = new CircleThrow();
            Utility = new Teleport();
            Ultimate = new Heal();
        }
    }
}
