using System;
using GentrysQuest.Game.Entity;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Location.Drawables;

public partial class DrawableMapObject : CompositeDrawable
{
    public IMapObject MapObjectReference { get; }
    public AffiliationType Affiliation { get; set; } = AffiliationType.None;

    [CanBeNull]
    public CollisonHitBox Collider { get; }

    public IntersectingHitBox IntersectingHitBox { get; set; }

    public DrawableMapObject(IMapObject mapObject)
    {
        MapObjectReference = mapObject ?? throw new ArgumentNullException(nameof(mapObject));

        if (mapObject is MapObject concreteMapObject && concreteMapObject.HasCollider)
            Collider = new CollisonHitBox(this);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativePositionAxes = MapObjectReference.RelativePositionAxes;
        RelativeSizeAxes = MapObjectReference.RelativeSizeAxes;
        Anchor = MapObjectReference.Anchor;
        Origin = MapObjectReference.Origin;
        Size = MapObjectReference.Size;
        Position = MapObjectReference.Position;
        Colour = MapObjectReference.Colour;

        AddInternal(IntersectingHitBox = new IntersectingHitBox(this));
        if (Collider != null) AddInternal(Collider);
        AddInternal(new Box
        {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both
        });
    }
}
