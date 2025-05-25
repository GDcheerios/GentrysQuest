using System;
using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class InventoryButton : GqButton
    {
        private SpriteText buttonText;
        private Action action;
        private readonly Box background;

        public InventoryButton(string text)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Masking = true;
            CornerExponent = 2;
            CornerRadius = 10;
            // BorderColour = Colour4.Black;
            // BorderThickness = 2;
            Size = new Vector2(200, 50);
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(42, 42, 42, 255)
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Child = buttonText = new SpriteText()
                    {
                        Text = text,
                        Colour = new Colour4(169, 169, 169, 255),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = FontUsage.Default.With(size: 36)
                    }
                }
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            background.FlashColour(Colour4.Gray, 500, Easing.In);
            return base.OnClick(e);
        }

        public void SetText(string text) => buttonText.Text = text;

        public void SetTextColor(Colour4 colour) => buttonText.Colour = colour;
    }
}
