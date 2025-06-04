using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqBackground : Box
    {
        public GqBackground()
        {
            RelativeSizeAxes = Axes.Both;
            Colour = Colour4.Gray;
            Depth = 1;
        }
    }
}
