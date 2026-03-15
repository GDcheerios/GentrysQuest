using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu
{
    public partial class GqMenuButton : MainGqButton
    {
        public GqMenuButton(string text)
            : base(text)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(200, 50);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }
    }
}
