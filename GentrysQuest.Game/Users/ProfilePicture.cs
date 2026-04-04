using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Users
{
    public partial class ProfilePicture : CompositeDrawable
    {
        public ProfilePicture()
        {
            Margin = new MarginPadding(10);
            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(255, 255, 255, 100),
                }
            ];
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(42);
            Masking = true;
            CornerRadius = 8;
            CornerExponent = 1.5f;
        }
    }
}
