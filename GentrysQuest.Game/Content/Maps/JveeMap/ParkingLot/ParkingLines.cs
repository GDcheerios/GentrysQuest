using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps.JveeMap.ParkingLot
{
    public partial class ParkingLines : MapObject
    {
        private int spots;
        private bool diagonal;
        private int angle;
        public const int LINE_WIDTH = 40;
        public const int LINE_SPACING = 15;

        private static readonly Colour4 line_colour = Colour4.Yellow;

        public ParkingLines(int spots, bool diagonal, int angle = 0)
        {
            this.spots = spots;
            this.diagonal = diagonal;
            this.angle = angle;
            Filled = false;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(
                MathBase.GetFeetToPixels(LINE_WIDTH),
                MathBase.GetFeetToPixels(LINE_SPACING * spots)
            );
            AddInternal(new MapObject
            {
                RelativeSizeAxes = Axes.Y,
                Size = new Vector2(LINE_WIDTH, 1),
                Colour = line_colour,
            });

            for (int i = 0; i < spots; i++)
            {
                AddInternal(new MapObject
                {
                    Size = new Vector2(1, (float)(LINE_WIDTH * 0.5)), // x is sideways
                    Rotation = angle,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Y = MathBase.GetFeetToPixels(LINE_SPACING * i),
                    Colour = line_colour,
                });
            }
        }
    }
}
