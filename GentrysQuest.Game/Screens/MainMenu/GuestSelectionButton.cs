using GentrysQuest.Game.Database;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class GuestSelectionButton : GQButton
    {
        private User linkedGuestUser;
        private Box background;
        private SpriteText guestName;
        private SpriteText levelText;

        public GuestSelectionButton(string guestName)
        {
            linkedGuestUser = GuestFileManager.GetGuestData(guestName).User;
            this.guestName = new SpriteText
            {
                Text = guestName,
                Colour = Colour4.White,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Margin = new MarginPadding { Left = 100 },
                Font = FontUsage.Default.With(size: 48),
                Shear = new Vector2(0.1f, 0),
            };
            levelText = new SpriteText
            {
                Text = $"Lvl. {linkedGuestUser.Level.ToString()}",
                Colour = Colour4.White,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 50 },
                Font = FontUsage.Default.With(size: 48)
            };

            SetAction(delegate
            {
                GameData.CurrentUser.Value = linkedGuestUser;
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            Masking = true;
            Margin = new MarginPadding { Top = 10 };
            CornerExponent = 1;
            CornerRadius = 100;
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(0.95f, 100);
            Children =
            [
                background = new Box
                {
                    Colour = new Colour4(24, 24, 24, 200),
                    RelativeSizeAxes = Axes.Both,
                },
                levelText,
                guestName
            ];
        }

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(new Colour4(50, 50, 50, 200), 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(new Colour4(24, 24, 24, 200), 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
