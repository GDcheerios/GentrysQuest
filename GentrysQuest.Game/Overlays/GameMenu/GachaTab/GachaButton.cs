using System;
using GentrysQuest.Game.Gachas;
using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class GachaButton : GqButton
    {
        private Gacha gacha;

        public GachaButton(Gacha gacha, Action action)
        {
            this.gacha = gacha;
            SetAction(action);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            CornerRadius = 10;
            CornerExponent = 2;
            BorderThickness = 5;
            BorderColour = Colour4.Black;
            Margin = new MarginPadding { Top = 10 };
            Children =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Gray,
                },
                new GqText(gacha.Name)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Left = 10 }
                },
                new GqText($"${gacha.Price}")
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Margin = new MarginPadding { Right = 10 },
                    Colour = Colour4.LightGray
                }
            ];
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.FlashColour(Colour4.LightGray, 500, Easing.In);
            return base.OnClick(e);
        }
    }
}
