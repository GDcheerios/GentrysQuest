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

        /// <summary>
        /// Loads map objects.
        /// </summary>
        public virtual void Load()
        {
            // implement map loading logic

            // Add barriers and floor
            // This is the default logic to prevent players from escaping,
            // But you do you

            Objects.Add(new MapObject{ Name = "test", Position = new Vector2(2232,224)});

            // top
            Objects.Add(new MapObject
            {
                RelativeSizeAxes = Axes.X,
                Size = new Vector2(1, 1),
                HasCollider = true,
                Filled = false
            });

            // bottom
            Objects.Add(new MapObject
            {
                RelativeSizeAxes = Axes.X,
                Size = new Vector2(1, 1),
                RelativePositionAxes = Axes.Y,
                Y = 1,
                HasCollider = true,
                Filled = false
            });
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
