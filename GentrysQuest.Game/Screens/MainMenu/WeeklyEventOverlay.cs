using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Families.BraydenMesserschmidt;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Overlays.Results;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class WeeklyEventOverlay : CompositeDrawable
    {
        private MainMenuButton playButton;
        private OnlineResultsLeaderboard leaderboard;
        private Container detailsContainer;
        private TaggedTextContainer descriptionText;

        private readonly string eventName;
        private readonly string eventDescription;
        private readonly int eventID = 1;

        public WeeklyEventOverlay()
        {
            playButton = new MainMenuButton("Play")
            {
                Size = new Vector2(300, 100),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
            };
            leaderboard = new OnlineResultsLeaderboard(eventID)
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.5f, 0.8f),
            };

            #region EventDetails

            eventName = "Brayden Gaming";
            eventDescription = "Play as Brayden's peak set. "
                               + "You start with [stat]5 star[/stat] Brayden Messerschmidt equipment, all at [stat]level 100[/stat]";

            playButton.SetAction(delegate
            {
                GameData.Store();

                Character character = new BraydenMesserschmidt();
                character.Experience.Level.Current.Value = 100;
                GameData.EquipCharacter(character);
                GameData.Add(character);
                BraydensOsuPen braydensOsuPen = new BraydensOsuPen();
                levelLoop(100, braydensOsuPen);
                character.SetWeapon(braydensOsuPen);

                OsuTablet osuTablet = new OsuTablet();
                osuTablet.Initialize(5);

                MadokaChibiPlush osuChibiPlush1 = new MadokaChibiPlush();
                osuChibiPlush1.Initialize(5);
                MadokaChibiPlush osuChibiPlush2 = new MadokaChibiPlush();
                osuChibiPlush2.Initialize(5);
                MadokaChibiPlush osuChibiPlush3 = new MadokaChibiPlush();
                osuChibiPlush3.Initialize(5);
                MadokaChibiPlush osuChibiPlush4 = new MadokaChibiPlush();
                osuChibiPlush4.Initialize(5);

                levelLoop(19, osuTablet);
                levelLoop(19, osuChibiPlush1);
                levelLoop(19, osuChibiPlush2);
                levelLoop(19, osuChibiPlush3);
                levelLoop(19, osuChibiPlush4);

                character.Artifacts.Equip(osuTablet, 0);
                character.Artifacts.Equip(osuChibiPlush1, 1);
                character.Artifacts.Equip(osuChibiPlush2, 2);
                character.Artifacts.Equip(osuChibiPlush3, 3);
                character.Artifacts.Equip(osuChibiPlush4, 4);
            });

            #endregion
        }

        private void levelLoop(int amount, EntityBase entity)
        {
            for (int i = 0; i < amount; i++) entity.LevelUp();
        }

        public void EndLeaderboard() => leaderboard.LeaderboardPanels.Clear();

        public void ReloadLeaderboard() => leaderboard.Load();

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren =
            [
                playButton,
                leaderboard,
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
