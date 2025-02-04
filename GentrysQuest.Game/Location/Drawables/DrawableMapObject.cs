using GentrysQuest.Game.Entity;
using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Location.Drawables;

public partial class DrawableMapObject : CompositeDrawable
{
    public IMapObject MapMapObjectReference { get; }
    public AffiliationType Affiliation { get; }

    [CanBeNull]
    public CollisonHitBox Collider { get; }

    public IntersectingHitBox IntersectingHitBox { get; }

    public DrawableMapObject(MapObject mapMapObject)
    {
        MapMapObjectReference = mapMapObject;
        Affiliation = AffiliationType.None;
        RelativePositionAxes = Axes.Both;
        RelativeSizeAxes = Axes.Both;
        Size = mapMapObject.Size;
        Position = mapMapObject.Position;
        Colour = mapMapObject.Colour;
        if (mapMapObject.HasCollider) Collider = new CollisonHitBox(this);

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both
            },
            Collider,
            IntersectingHitBox = new IntersectingHitBox(this)
        };
    }

    public DrawableMapObject(MapZone mapMapZone)
    {
        MapMapObjectReference = mapMapZone;
        Affiliation = AffiliationType.None;
        RelativePositionAxes = Axes.Both;
        RelativeSizeAxes = Axes.Both;
        Size = mapMapZone.Size;
        Position = mapMapZone.Position;
        Colour = mapMapZone.Colour;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both
            },
            IntersectingHitBox = new IntersectingHitBox(this)
        };
    }
}
