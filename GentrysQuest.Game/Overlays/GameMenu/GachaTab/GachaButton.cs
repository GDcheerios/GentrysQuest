using System;
using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class GachaButton : GqButton
    {
        private readonly string text;

        public GachaButton(string text, Action action)
        {
            this.text = text;
            SetAction(action);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            CornerRadius = 10;
            CornerExponent = 2;
            BorderThickness = 2;
            BorderColour = Colour4.Black;
            Child = new GqText(text)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
        }
    }
}
