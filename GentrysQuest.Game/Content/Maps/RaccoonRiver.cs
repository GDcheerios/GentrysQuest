using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Content.Maps
{
    public class RaccoonRiver : Map
    {
        public RaccoonRiver()
        {
            Name = "Raccoon River";
            Size = new Vector2(
                MathBase.GetMilesToPixels(0.5),
                MathBase.GetMilesToPixels(1)
            );
        }
    }
}
