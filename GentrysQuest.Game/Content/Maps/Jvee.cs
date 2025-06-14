using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class Jvee : Map
    {
        public Jvee()
        {
            Name = "J-Vee";
            Size = new
                Vector2(
                    MathBase.GetFeetToPixels(500)
                );
        }

        public override void Load()
        {
            base.Load();

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
        }
    }
}
