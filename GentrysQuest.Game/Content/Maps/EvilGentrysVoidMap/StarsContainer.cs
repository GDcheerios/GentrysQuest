using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osuTK;

namespace GentrysQuest.Game.Content.Maps.EvilGentrysVoidMap
{
    public partial class StarsContainer : MapObject
    {
        private const int star_count = 20;

        public StarsContainer()
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            for (int i = 0; i < star_count; i++)
            {
                AddInternal(new MapObject
                {
                    Size = new Vector2(MathBase.RandomFloat(5, 20)),
                    Position = new Vector2(MathBase.RandomInt(-50, 50), MathBase.RandomInt(-50, 50))
                });
            }
        }
    }
}
