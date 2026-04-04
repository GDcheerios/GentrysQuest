using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Overlays.PlayerSelect
{
    public partial class GuestSelectionButton : GqButton
    {
        private readonly GuestUser linkedGuestUser;
        private Box underline;
        private readonly SpriteText guestName;
        private readonly SpriteText levelText;
        private readonly GuestDeleteButton deleteButton;

        [Resolved]
        private Bindable<IUser> currentUser { get; set; }

        public GuestSelectionButton(string guestName)
        {
            linkedGuestUser = new GuestUser(guestName);
            linkedGuestUser.Load();
            this.guestName = new SpriteText
            {
                Text = guestName,
                Colour = Colour4.White,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Font = FontUsage.Default.With(size: 28),
                Margin = new MarginPadding { Left = 5 },
            };
            levelText = new SpriteText
            {
                Text = $"{linkedGuestUser.Experience.Level}",
                Colour = Colour4.White,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 25 },
                Font = FontUsage.Default.With(size: 20)
            };
            deleteButton = new GuestDeleteButton
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Left = 5 },
                Size = new Vector2(25)
            };
            deleteButton.SetAction(delegate
            {
                linkedGuestUser.Delete();
                this.ScaleTo(new Vector2(0, 1), 200, Easing.In);
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(0.95f, 60);
            Children =
            [
                new Container
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 0.1f),
                        },
                        underline = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.3f, 0.1f),
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Colour = Colour4.White
                        }
                    ]
                },
                levelText,
                guestName,
                deleteButton
            ];
        }

        protected override bool OnClick(ClickEvent e)
        {
            currentUser.Value = linkedGuestUser;
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            underline.ResizeTo(new Vector2(0.5f, 0.1f), 200, Easing.Out);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            underline.ResizeTo(new Vector2(0.3f, 0.1f), 100, Easing.In);
            base.OnHoverLost(e);
        }
    }
}
