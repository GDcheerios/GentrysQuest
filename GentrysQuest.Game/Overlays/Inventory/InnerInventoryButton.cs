using System;
using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class InnerInventoryButton : GqButton
    {
        private readonly Box backgroundBox;
        public readonly SpriteText Text;
        public event EventHandler OnClickEvent;

        public InnerInventoryButton(SpriteText text = null)
        {
            Masking = true;
            CornerExponent = 2;
            CornerRadius = 7;
            InternalChildren = new Drawable[]
            {
                backgroundBox = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Alpha = 0.17f
                },
                Text = text ?? new SpriteText()
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            backgroundBox.FadeTo(0.36f, 50);
            // this.ScaleTo(1.1f, 50);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            backgroundBox.FadeTo(0.17f, 50);
            // this.ScaleTo(1, 50);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            OnClickEvent?.Invoke(null, null);
            return base.OnClick(e);
        }
    }
}
