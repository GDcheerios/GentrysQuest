using GentrysQuest.Game.Database;
using GentrysQuest.Game.Overlays.LoginOverlay;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class PlayerSelectBox : CompositeDrawable
    {
        private LoginOverlay loginOverlay;
        private MainMenuButton playButton;
        private MainMenuButton backButton;

        public PlayerSelectBox(MainMenu mainMenuScreen)
        {
            playButton = new MainMenuButton("Play")
            {
                Margin = new MarginPadding { Bottom = 10 },
                Scale = new Vector2(1, 0),
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Size = new Vector2(400, 75)
            };
            backButton = new MainMenuButton("Back")
            {
                Margin = new MarginPadding { Bottom = 10 },
                Scale = new Vector2(1, 1),
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Size = new Vector2(400, 75)
            };

            GameData.CurrentUser.ValueChanged += delegate
            {
                if (GameData.CurrentUser.Value != null)
                {
                    playButton.ScaleTo(Vector2.One, 200, Easing.OutQuint);
                    backButton.ScaleTo(new Vector2(1, 0), 200, Easing.OutQuint);
                }
                else
                {
                    playButton.ScaleTo(new Vector2(1, 0), 200, Easing.OutQuint);
                    backButton.ScaleTo(new Vector2(1, 1), 200, Easing.OutQuint);
                }
            };

            playButton.SetAction(mainMenuScreen.EnterSelection);
            backButton.SetAction(mainMenuScreen.PressBack);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = 16,
                CornerExponent = 2,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Gray
                    },
                    new SpriteText
                    {
                        Text = "Please Login",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Colour = Colour4.Black,
                        Font = FontUsage.Default.With(size: 42)
                    },
                    loginOverlay = new LoginOverlay
                    {
                        Scale = new Vector2(0.8f, 0.8f),
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                    },
                    playButton,
                    backButton
                }
            });
        }
    }
}
