using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Overlays.Results;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace GentrysQuest.Game.Overlays.GameMenu
{
    public partial class EventOverlay : CompositeDrawable
    {
        private readonly MainGqButton playButton;
        private readonly OnlineResultsLeaderboard leaderboard;
        private Container detailsContainer;
        private TaggedTextContainer descriptionText;

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private Bindable<IUser> user { get; set; }

        private readonly string eventName;
        private readonly string eventDescription;
        public const int EVENT_ID = 1;

        public EventOverlay()
        {
            playButton = new MainGqButton("Play")
            {
                Size = new Vector2(300, 100),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
            };

            #region EventDetails

            eventName = "March Gameplay Test";
            eventDescription = "This is a gameplay testing event for the month of march.";

            playButton.SetAction(delegate
            {
                if (user.Value != null)
                    user.Value.SessionMode = UserSessionMode.Event;

                GMoney character = new GMoney();
                user.Value!.AddItem(character);
                user.Value.EquippedCharacter = character;
                character.SetWeapon(new Sword());

                EventGameplayScreen eventScreen = new EventGameplayScreen();

                screenManager.SetScreen(eventScreen);
                eventScreen.LoadGameplay(user.Value, new TestMap());
            });

            #endregion
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren =
            [
                playButton,
                detailsContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    Size = new Vector2(0.5f, 0.8f),
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        descriptionText = new TaggedTextContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Y = 50,
                            Padding = new MarginPadding(5),
                            Colour = Colour4.Black,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                        },
                        new SpriteText
                        {
                            Text = eventName,
                            Colour = Colour4.Black,
                            Font = FontUsage.Default.With(size: 30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        }
                    ]
                }
            ];
            RelativeSizeAxes = Axes.Both;
            descriptionText.SetTaggedText(eventDescription);
        }
    }
}
