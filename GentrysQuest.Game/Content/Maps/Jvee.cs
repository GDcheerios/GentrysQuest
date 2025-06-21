using GentrysQuest.Game.Content.Maps.JveeMap.ParkingLot;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class Jvee : Map
    {
        private static readonly int[] HORIZONTAL_PARKING =
        [
            -30,
            30,
            -30,
            -30,
            30,
            -30,
            30,
            -30
        ];

        public Jvee()
        {
            Name = "J-Vee";
            Size = new
                Vector2(
                    MathBase.GetFeetToPixels(540),
                    MathBase.GetFeetToPixels(530)
                );
            SpawnPoint = new Vector2(0, MathBase.GetFeetToPixels(-500));
        }

        public override void Load()
        {
            base.Load();

            Objects.Add(new MapObject{Name="test", Position = new Vector2(100, 100), Size = new Vector2(100, 100)});

            #region ParkingLot

            Objects.Add(new MapObject
            {
                Name = "Concrete",
                Colour = Colour4.Gray,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre
            });

            Objects.Add(new MapObject
            {
                Colour = Colour4.Brown,
                X = MathBase.GetFeetToPixels(500 * 0.5),
                Y = MathBase.GetFeetToPixels(500 * 0.5),
                Size = new Vector2(
                    MathBase.GetFeetToPixels(24),
                    MathBase.GetFeetToPixels(12)
                )
            });

            int startX = (int)(24 + ParkingLines.LINE_WIDTH * 0.5);

            for (int i = 0; i < HORIZONTAL_PARKING.Length; i++)
            {
                Objects.Add(new ParkingLines(20, true, HORIZONTAL_PARKING[i])
                {
                    X = MathBase.GetFeetToPixels(startX + (i * (ParkingLines.LINE_WIDTH + 24)))
                });
            }

            #endregion
        }
    }
}
