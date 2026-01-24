using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    /// <summary>
    /// A flashy container to display items
    /// </summary>
    public partial class FlashyItemDisplay : Container
    {
        private Circle circle;

        [BackgroundDependencyLoader]
        private void load()
        {
            FillMode = FillMode.Fit;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Child = circle = new Circle
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.DarkGray,
                Size = Vector2.Zero
            };
        }

        public void SetupFlash(int starRating)
        {
            circle.ResizeTo(new Vector2(0.2f * starRating), 100);
        }
    }
}
