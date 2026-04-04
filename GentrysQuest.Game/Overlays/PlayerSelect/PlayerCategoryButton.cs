using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Overlays.PlayerSelect
{
    public partial class PlayerCategoryButton : GqButton
    {
        private string text;

        public PlayerCategoryButton(string text) => this.text = text;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.065f);
            InternalChildren =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.None,
                            Size = new Vector2(text.Length * 12, 5),
                            Y = 5,
                            Colour = Colour4.White,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.TopCentre,
                        },
                        new SpriteText
                        {
                            Text = text,
                            Colour = Colour4.White,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Margin = new MarginPadding { Top = 2 },
                            Font = FontUsage.Default.With(size: 32)
                        }
                    ]
                },
            ];
        }

        protected override bool OnHover(HoverEvent e)
        {
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
        }
    }
}
