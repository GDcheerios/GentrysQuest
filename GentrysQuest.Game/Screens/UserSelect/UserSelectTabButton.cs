using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Screens.UserSelect
{
    public partial class UserSelectTabButton : GQButton
    {
        private Box backrground;

        public UserSelectTabButton(bool isGuest)
        {
            Size = new Vector2(190, 100);
            InternalChildren = new Drawable[]
            {
                backrground = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Gray
                },
                new SpriteText
                {
                    Text = (isGuest ? "Guest" : "Login"),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            backrground.Colour = Colour4.LightGray;
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            backrground.Colour = Colour4.DarkGray;
        }
    }
}
