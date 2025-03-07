using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens
{
    public partial class GqScreen : Screen
    {
        protected readonly Container Overlay = new()
        {
            RelativeSizeAxes = Axes.Both,
            Depth = -100
        };

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(Overlay);
        }
    }
}
