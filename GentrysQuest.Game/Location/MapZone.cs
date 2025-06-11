using System;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public abstract partial class MapZone : MapObject
    {
        public override Vector2 Size { get; set; } = Vector2.One;
        public new Vector2 Position { get; set; }
        public new EventHandler OnTouchEvent { get; set; }
        public new Anchor Anchor { get; set; } = Anchor.Centre;
        public override Anchor Origin { get; set; } = Anchor.TopLeft;
        public override Axes RelativeSizeAxes { get; set; } = Axes.None;
        public new Axes RelativePositionAxes { get; set; } = Axes.None;

        public virtual void OnTouch()
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

