using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class MekhiElliot : Character
    {
        public MekhiElliot()
        {
            Name = "Mekhi Elliot";
            StarRating = new StarRating(5);
            Description = "Big biggest and baddest gentry warrior the biggest alpha sigma of them all.";

            TextureMapping.Add("Idle", "characters_mekhi_idle.jpg");
        }
    }
}
