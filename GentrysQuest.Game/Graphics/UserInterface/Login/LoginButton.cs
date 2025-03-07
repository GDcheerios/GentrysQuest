using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Graphics.UserInterface.Login
{
    public partial class LoginButton : GqButton
    {
        private SpriteText text;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both
            });
            Add(text = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 24),
                Text = "Login"
            });
        }

        public void SetText(string text) => this.text.Text = text;
    }
}
