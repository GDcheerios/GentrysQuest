using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class MainMenuButton : GQButton
    {
        private Box background;
        private SpriteText text;

        public MainMenuButton(string text)
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 15,
                    CornerExponent = 2,
                    Child = background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = ColourInfo.GradientVertical(
                            new Colour4(58, 58, 58, 255),
                            Colour4.Black
                        )
                    }
                },
                this.text = new SpriteText
                {
                    Text = text,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = FontUsage.Default.With(size: 24),
                    Colour = Colour4.White
                }
            };
        }

        public void SetText(string text) => this.text.Text = text;

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(ColourInfo.GradientVertical(
                new Colour4(72, 72, 72, 255),
                Colour4.Black
            ), 100);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(ColourInfo.GradientVertical(
                new Colour4(58, 58, 58, 255),
                Colour4.Black
            ), 100);
            base.OnHoverLost(e);
        }
    }
}
