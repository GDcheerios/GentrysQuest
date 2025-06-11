using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public class Map
    {
        public string Name { get; protected set; }
        public List<Family> Families { get; } = new();
        public List<MapObject> Objects { get; } = new();
        public List<DrawableEntity> Npcs { get; } = new();
        public Vector2 Size { get; protected set; } = Vector2.Zero;
        public Vector2 SpawnPoint { get; protected set; } = Vector2.Zero;

        public virtual void Load()
        {
            // implement map loading logic

            // Add barriers
            // This is the default logic to prevent players from escaping,
            // But you do you
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(Size.X * 2, 10), Position = new Vector2(-Size.X, Size.Y), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(Size.X * 2, 10), Position = new Vector2(-Size.X, -Size.Y), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(10, Size.Y * 2), Position = new Vector2(Size.X, -Size.Y), Colour = Colour4.Black });
            Objects.Add(new MapObject { HasCollider = true, Size = new Vector2(10, Size.Y * 2), Position = new Vector2(-Size.X, -Size.Y), Colour = Colour4.Black });
        }

        public int Difficulty { get; protected set; } = 0;
        public bool DifficultyScales { get; protected set; } = false;

        /// <summary>
        /// Code that runs every frame
        /// </summary>
        public virtual void Update()
        {
            // implement map update logic
        }
    }
}
