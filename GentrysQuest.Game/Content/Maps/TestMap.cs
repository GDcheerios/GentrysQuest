using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class TestMap : Map
    {
        public override void Load()
        {
            Name = "Test Map";
            DifficultyScales = true;

            for (int i = 0; i < 100; i++) Objects.Add(new MapObject { Size = getRandVec(0.01f, 0.1f), Position = getRandVec(0, 1) });

            // walls
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(1, 0.01f), Position = new Vector2(0, 0), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(1, 0.01f), Position = new Vector2(0, 1), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(0.01f, 1), Position = new Vector2(0, 0), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(0.01f, 1), Position = new Vector2(1, 0), Colour = Colour4.Black });
        }

        private Vector2 getRandVec(float min, float max)
        {
            return new Vector2(MathBase.RandomFloat(min, max), MathBase.RandomFloat(min, max));
        }
    }
}
