using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class TestMap : Map
    {
        public TestMap()
        {
            Name = "Test Map";
            DifficultyScales = true;
            Size = new Vector2(2000);
        }

        public override void Load()
        {
            base.Load();
            for (int i = 0; i < 100; i++) Objects.Add(new MapObject { HasCollider = true, Size = getRandVec(10, 300), Position = getRandVec(-2000, 2000) });
        }

        private Vector2 getRandVec(float min, float max)
        {
            return new Vector2(MathBase.RandomFloat(min, max), MathBase.RandomFloat(min, max));
        }
    }
}
