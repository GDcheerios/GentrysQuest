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
            Shear = new Vector2(0.3f, 0);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void HideButton()
        {
            this.ScaleTo(0);
            this.FadeOut(0);
        }

        public void ShowButton()
        {
            this.ScaleTo(1);
            this.FadeIn(0);
        }
    }
}
