using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Graphics;
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

namespace GentrysQuest.Game.Overlays.GameMenu
{
    public partial class EventOverlay : CompositeDrawable
    {
        private readonly MainGqButton playButton;
        private readonly MainGqButton endButton;
        private EventGameplayScreen eventScreen;
        private OnlineResultsLeaderboard resultsLeaderboard;
        private EventStatisticsOverlay statisticsOverlay;

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private Bindable<IUser> user { get; set; }

        private readonly string eventName;
        public const int EVENT_ID = 4;

        public EventOverlay()
        {
            playButton = new MainGqButton("Play")
            {
                Size = new Vector2(300, 100),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre
            };

            endButton = new MainGqButton("End")
            {
                Size = new Vector2(300, 100),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre
            };

            #region EventDetails

            eventName = "April Gameplay Test";

            #endregion
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren =
            [
                playButton,
                endButton,
                new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Text = eventName,
                    Font = new FontUsage(size: 48),
                },
                new Container
                {
                    Masking = true,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.65f),
                    Child = resultsLeaderboard = new OnlineResultsLeaderboard { ScoreLeaderboard = true }
                },
                new Container
                {
                    Masking = true,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.65f),
                    Child = statisticsOverlay = new EventStatisticsOverlay()
                }
            ];
            RelativeSizeAxes = Axes.Both;
            endButton.Hide();
            playButton.SetAction(delegate
            {
                playButton.Hide();
                endButton.Show();
                if (user.Value == null)
                    return;

                EventSessionInventoryScope.Begin(user.Value);

                showActiveControls();

                #region InventorySetup

                TestCharacter character = new TestCharacter(1);
                user.Value.Characters.Add(character);
                user.Value.EquippedCharacter = character;
                user.Value.AddItem(new Bow());
                user.Value.AddItem(new Hammer());
                user.Value.AddItem(new Spear());
                character.SetWeapon(new Sword());

                #endregion

                eventScreen = new EventGameplayScreen(EVENT_ID);
                eventScreen.ScoreSubmitted += UpdateEvent;
                eventScreen.GameplayEnded += onGameplayEnded;

                screenManager.SetScreen(eventScreen);
                eventScreen.LoadGameplay(user.Value, new TestMap());
            });

            endButton.SetAction(delegate
            {
                if (user.Value == null || eventScreen == null)
                    return;

                playButton.Show();
                endButton.Hide();
                _ = eventScreen.EndAsync(GameplayEndReason.EventEnded);
            });

            UpdateEvent();
        }

        public void UpdateEvent()
        {
            resultsLeaderboard.Load(EVENT_ID);
            statisticsOverlay.Load(EVENT_ID);
        }

        private void showActiveControls()
        {
            endButton.Show();
        }

        private void onGameplayEnded(GameplayEndReason reason)
        {
            if (reason != GameplayEndReason.Death)
                return;

            playButton.Show();
            endButton.Hide();
        }
    }
}
