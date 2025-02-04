using GentrysQuest.Game.Location;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class ValleyHighSchool : Map
    {
        public override void Load()
        {
            Name = "Intro";
            DifficultyScales = false;

            Objects.Add(new MapObject { Size = new Vector2(100, 10) });
        }
    }
}
