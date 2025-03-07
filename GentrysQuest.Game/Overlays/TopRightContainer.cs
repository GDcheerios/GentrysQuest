using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Overlays
{
    public partial class TopRightContainer : Container
    {
        private readonly FillFlowContainer contents;

        public TopRightContainer()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Depth = -3;
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(300);
            Child = contents = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.X,
                Height = 100,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            };
        }

        public void AddOverlay(Drawable drawable)
        {
            contents.Add(drawable);
        }
    }
}
