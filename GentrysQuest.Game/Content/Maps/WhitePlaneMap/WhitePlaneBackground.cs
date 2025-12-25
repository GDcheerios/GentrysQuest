using GentrysQuest.Game.Location;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Content.Maps.WhitePlaneMap
{
    public partial class WhitePlaneBackground : MapObject
    {
        private Box content;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChild = content = new Box
            {
                Colour = Colour4.White,
                RelativeSizeAxes = Axes.Both
            };
            content.FadeColour(Colour4.LightGray, 3500, Easing.In).Then()
                   .FadeColour(Colour4.White, 3500, Easing.In)
                   .Loop();
        }
    }
}
