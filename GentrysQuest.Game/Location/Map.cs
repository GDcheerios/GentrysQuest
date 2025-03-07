using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public class Map
    {
        public string Name { get; protected set; }
        public List<Family> Families { get; } = new();
        public List<IMapObject> Objects { get; } = new();
        public List<DrawableEntity> Npcs { get; } = new();
        public Vector2 Size { get; protected set; } = Vector2.Zero;

        public virtual void Load()
        {
            // implement map loading logic
        }

        public int Difficulty { get; protected set; } = 0;
        public bool DifficultyScales { get; protected set; } = false;
    }
}
