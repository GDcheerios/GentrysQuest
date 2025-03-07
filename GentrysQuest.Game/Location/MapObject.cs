using System;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Location;

public partial class MapObject : IMapObject
{
    public bool HasCollider { get; set; } = false;
    public Vector2 Size { get; set; } = Vector2.One;
    public EventHandler OnTouchEvent { get; set; } = null;
    public Anchor Anchor { get; set; } = Anchor.Centre;
    public Anchor Origin { get; set; } = Anchor.TopLeft;
    public Axes RelativeSizeAxes { get; set; } = Axes.None;
    public Axes RelativePositionAxes { get; set; } = Axes.None;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Colour4 Colour { get; set; } = Colour4.Black;
    public int Health { get; set; } = 10000;
    public int Hardness { get; set; } = 1;
}
