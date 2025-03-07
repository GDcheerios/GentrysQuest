using System;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public abstract class MapZone : IMapObject
    {
        public Vector2 Size { get; set; } = Vector2.One;
        public Vector2 Position { get; set; }
        public EventHandler OnTouchEvent { get; set; }
        public Anchor Anchor { get; set; } = Anchor.Centre;
        public Anchor Origin { get; set; } = Anchor.TopLeft;
        public Axes RelativeSizeAxes { get; set; } = Axes.None;
        public Axes RelativePositionAxes { get; set; } = Axes.None;

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

