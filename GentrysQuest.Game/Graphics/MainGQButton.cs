using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Graphics
{
    public partial class MainGqButton : GqButton
    {
        public virtual string Text { get; set; }

        public Container Container;

        public MainGqButton(string text) { Text = text; }
        public MainGqButton() { Text = ""; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(Container = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 10,
                CornerExponent = 1.5f,
                BorderThickness = 2,
                BorderColour = Colour4.Black,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 10,
                    Roundness = 0,
                    Hollow = true,
                    Offset = new Vector2(0, 4),
                    Colour = new Colour4(0, 0, 0, 200),
                },
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Colour4(157, 157, 157, 255),
                        Depth = 1
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = new SpriteText
                        {
                            Text = Text,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = FontUsage.Default.With(size: 32),
                            AllowMultiline = false,
                            Padding = new MarginPadding(10),
                        }
                    }
                ]
            });
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Container.ScaleTo(0.95f, 200, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);
            Container.ScaleTo(1f, 100, Easing.OutQuint);
        }

        protected override bool OnHover(HoverEvent e)
        {
            Container.ScaleTo(1.05f, 100, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Container.ScaleTo(1f, 100, Easing.OutQuint);
        }
    }
}
