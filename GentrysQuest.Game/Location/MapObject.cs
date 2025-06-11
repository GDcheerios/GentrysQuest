using System;
using GentrysQuest.Game.Entity;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Location;

public partial class MapObject : CompositeDrawable
{
    public bool HasCollider { get; set; } = false;
    public override Vector2 Size { get; set; } = Vector2.One;
    public EventHandler OnTouchEvent { get; set; } = null;
    public new Anchor Anchor { get; set; } = Anchor.Centre;
    public override Anchor Origin { get; set; } = Anchor.TopLeft;
    public override Axes RelativeSizeAxes { get; set; } = Axes.None;
    public new Axes RelativePositionAxes { get; set; } = Axes.None;
    public new Vector2 Position { get; set; } = Vector2.Zero;
    public int Health { get; set; } = 10000;
    public int Hardness { get; set; } = 1;

    [CanBeNull]
    public CollisonHitBox Collider { get; }

    public IntersectingHitBox IntersectingHitBox { get; set; }

    public AffiliationType Affiliation = AffiliationType.None;

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(IntersectingHitBox = new IntersectingHitBox(this));
        if (Collider != null) AddInternal(Collider);
        AddInternal(new Box
        {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both
        });
    }
}
