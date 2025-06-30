using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
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
            SpawnPoint = GetCoordinatePercent(0.5f, 0.5f);
        }

        public override void Load()
        {
            base.Load();
            for (int i = 0; i < 100; i++) Objects.Add(new MapObject { HasCollider = true, Colour = Colour4.Black, Size = getRandVec(10, 300), Position = getRandVec(0, Size.X * 2) });
        }

        private Vector2 getRandVec(float min, float max) => new(MathBase.RandomFloat(min, max), MathBase.RandomFloat(min, max));
    }
}
