using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class EvilGentry : Enemy
    {
        public EvilGentry()
        {
            Name = "Evil Gentry";
            Description = "Mr. Gentry’s evil twin brother who hates frisbee golf.";

            Stats.Speed.Point = 2;
            Stats.Tenacity.Point = 10;

            TextureMapping.Add("Idle", "enemies_gmoney_idle.png");
        }
    }
}
