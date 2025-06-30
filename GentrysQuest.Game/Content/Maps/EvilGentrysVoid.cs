using GentrysQuest.Game.Content.Maps.EvilGentrysVoidMap;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class EvilGentrysVoid : Map
    {
        private static readonly Colour4 barrier_colour = new(12, 12, 12, 255);

        public EvilGentrysVoid()
        {
            Name = "Evil Gentry's Void";
            Size = new Vector2(MathBase.GetFeetToPixels(500));
            SpawnPoint = GetCoordinatePercent(0.5f, 0.5f);
        }

        public override void Load()
        {
            Objects.Add(new EvilGentrysVoidBackground());
            Objects.Add(new MapObject
            {
                Colour = barrier_colour,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.4f, 1),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                HasCollider = true
            });
            Objects.Add(new MapObject
            {
                Colour = barrier_colour,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.4f, 1),
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                HasCollider = true
            });
            Objects.Add(new MapObject
            {
                Colour = barrier_colour,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.495f),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                HasCollider = true
            });
        }
    }
}
