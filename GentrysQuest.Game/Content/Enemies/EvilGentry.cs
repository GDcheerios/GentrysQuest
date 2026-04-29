using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.AI;

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
            AiProfile = AiProfile.Aggressive();

            TextureMapping = new();
            TextureMapping.Add("Idle", "enemies_gmoney_idle.png");
        }
    }
}
