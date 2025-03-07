using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.PlayerSelect
{
    public partial class GuestDeleteButton : GqButton
    {
        private SpriteIcon icon;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = icon = new SpriteIcon
            {
                Icon = FontAwesome.Solid.TrashAlt,
                Colour = Colour4.White,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            icon.FadeColour(Colour4.Red, 100, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            icon.FadeColour(Colour4.White, 100, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
