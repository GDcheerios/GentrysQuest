using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.PlayerSelect
{
    public partial class PlayerSelectButton : GqButton
    {
        private readonly string text;
        private Box background;
        private static readonly Colour4 REG_COLOUR = new Colour4(177, 177, 177, 255);
        private static readonly Colour4 HOVER_COLOUR = new Colour4(200, 200, 200, 255);

        public PlayerSelectButton(string text) => this.text = text;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new Container
            {
                Masking = true,
                CornerExponent = 2,
                CornerRadius = 10,
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = REG_COLOUR,
                        Alpha = 0.2f,
                    },
                    new SpriteText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Colour4.White
                    }
                ]
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(HOVER_COLOUR, 100, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(REG_COLOUR, 100, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
