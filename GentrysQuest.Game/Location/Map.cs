using System.Collections.Generic;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Location
{
    public class Map
    {
        public string Name { get; protected set; }
        public List<Family> Families { get; } = new();
        public List<MapObject> Objects { get; } = new();

        public virtual void Load()
        {
            // implement map loading logic
        }

        public int Difficulty { get; protected set; } = 0;
        public bool DifficultyScales { get; protected set; } = false;
    }
}
