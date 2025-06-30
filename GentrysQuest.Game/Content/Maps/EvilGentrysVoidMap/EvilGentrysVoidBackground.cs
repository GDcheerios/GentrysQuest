using GentrysQuest.Game.Location;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Content.Maps.EvilGentrysVoidMap
{
    public partial class EvilGentrysVoidBackground : MapObject
    {
        public EvilGentrysVoidBackground()
        {
            Colour = new Colour4(12, 12, 12, 255);
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            this.FadeColour(new Colour4(42, 42, 42, 255), 10000, Easing.Out).Then()
                .FadeColour(new Colour4(12, 12, 12, 255), 10000, Easing.In).Loop();
        }
    }
}
