using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location.Drawables;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public class Map
    {
        public string Name { get; protected set; }
        public List<MapObject> Objects { get; } = new();
        public List<DrawableEntity> Npcs { get; } = new();
        public Vector2 Size { get; protected set; } = Vector2.Zero;
        public Vector2 SpawnPoint { get; protected set; } = Vector2.Zero;
        private DrawableMap drawableInstance = null;

        /// <summary>
        /// Loads map objects.
        /// </summary>
        public virtual void Load()
        {
            // implement map loading logic

            // Add barriers and floor
            // This is the default logic to prevent players from escaping,
            // But you do you

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
                Anchor = Anchor.BottomLeft,
                HasCollider = true,
                Filled = false
            });

            // left
            Objects.Add(new MapObject
            {
                RelativeSizeAxes = Axes.Y,
                Size = new Vector2(1, 1),
                Anchor = Anchor.TopLeft,
                HasCollider = true,
                Filled = false
            });

            // right
            // left
            Objects.Add(new MapObject
            {
                RelativeSizeAxes = Axes.Y,
                Size = new Vector2(1, 1),
                Anchor = Anchor.TopRight,
                HasCollider = true,
                Filled = false
            });
        }

        public void SetDrawable(DrawableMap drawable) => drawableInstance = drawable;
        public DrawableMap GetDrawable() => drawableInstance;

        public int Difficulty { get; protected set; } = 0;
        public bool DifficultyScales { get; protected set; } = false;

        /// <summary>
        /// Get coordinates based off percentage.
        /// </summary>
        /// <param name="x">X percentage</param>
        /// <param name="y">Y percentage</param>
        /// <returns>Vector2 coordinates</returns>
        public Vector2 GetCoordinatePercent(float x, float y) => new(x * (Size.X * 2), y * (Size.Y * 2));
    }
}
