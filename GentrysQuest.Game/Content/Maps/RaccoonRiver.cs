using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class RaccoonRiver : Map
    {
        /// <summary>
        /// This is to cover the map to provide
        /// an effect of daytime/nighttime.
        /// </summary>
        private IMapObject timeCover;

        public RaccoonRiver()
        {
            Name = "Raccoon River";
            Size = new Vector2(
                MathBase.GetMilesToPixels(0.5),
                MathBase.GetMilesToPixels(1)
            );
            SpawnPoint = new Vector2();
        }

        public override void Load()
        {
            base.Load();

            Objects.Add(new MapObject
            {
                HasCollider = false,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Blue
            });

            // keep at the bottom of the load method
            Objects.Add(timeCover = new MapObject
            {
                Colour = new Colour4(0, 0, 0, 10),
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
        }
    }
}
