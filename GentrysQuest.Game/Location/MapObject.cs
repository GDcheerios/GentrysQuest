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
    public bool HasCollider { get; set; }
    public override Vector2 Size { get; set; } = Vector2.One;
    public EventHandler OnTouchEvent { get; set; } = null;
    public new Anchor Anchor { get; set; } = Anchor.Centre;
    public override Anchor Origin { get; set; } = Anchor.TopLeft;
    public override Axes RelativeSizeAxes { get; set; } = Axes.None;
    public new Axes RelativePositionAxes { get; set; } = Axes.None;
    public int Health { get; set; } = 10000;
    public int Hardness { get; set; } = 1;
    public bool Filled { get; set; } = true;
    public bool Reactive { get; set; } = false;

    [CanBeNull]
    public CollisonHitBox Collider { get; set; }

    public IntersectingHitBox IntersectingHitBox { get; set; }

    public AffiliationType Affiliation = AffiliationType.None;

    [BackgroundDependencyLoader]
    private void load()
    {
        if (Filled)
        {
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
            });
        }

        if (Reactive) AddInternal(IntersectingHitBox = new IntersectingHitBox(this));
        if (HasCollider) AddInternal(Collider = new CollisonHitBox(this));
    }
}
