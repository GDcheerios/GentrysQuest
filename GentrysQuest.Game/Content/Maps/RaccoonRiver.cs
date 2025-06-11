using GentrysQuest.Game.Content.Maps.RaccoonRiverMap;
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
        private MapObject timeCover;

        private const int DAY_CYCLE_TIME = 100 * 1000;
        private const int DAY_STRENGTH = 10;
        private const int NIGHT_STRENGTH = 180;

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

            # region Prerequisites

            Objects.Add(timeCover = new TimeCover(DAY_CYCLE_TIME, DAY_STRENGTH, NIGHT_STRENGTH));

            # endregion

            Objects.Add(new MapObject
            {
                HasCollider = false,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Blue
            });
        }
    }
}
