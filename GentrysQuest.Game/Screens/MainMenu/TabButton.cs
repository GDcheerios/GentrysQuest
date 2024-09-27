using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class TabButton : GQButton
    {
        private string text;
        private Box background;

        public TabButton(string text) => this.text = text;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.065f);
            Y = 15;
            InternalChildren =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 5,
                    CornerExponent = 2,
                    Children =
                    [
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        new SpriteText
                        {
                            Text = text,
                            Colour = Colour4.Black,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Margin = new MarginPadding { Top = 2 },
                            Font = FontUsage.Default.With(size: 32),
                        }
                    ]
                },
            ];
        }

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(Colour4.LightGray, 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(Colour4.Gray, 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
