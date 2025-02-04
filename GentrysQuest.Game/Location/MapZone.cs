using System;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public abstract class MapZone : IMapObject
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public EventHandler OnTouchEvent { get; set; }
        public void OnTouch()
        {
            throw new NotImplementedException();
        }

        public abstract Colour4 Colour { get; set; }
#if DEBUG
        public float Alpha { get; set; } = 0.5f;
#else
        public float Alpha { get; set; } = 0;
#endif
    }
}

