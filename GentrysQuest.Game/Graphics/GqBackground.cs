using GentrysQuest.Game.Graphics.TextStyles;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqBackground : CompositeDrawable
    {
        public GqBackground() => Depth = 1;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ColourInfo.GradientVertical(Colour4.LightGray, Colour4.DarkGray)
                },
                new TitleText("Gentry's Quest Test") { Origin = Anchor.TopCentre, Anchor = Anchor.TopCentre }
            ];
        }
    }
}
