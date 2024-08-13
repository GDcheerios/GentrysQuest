using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Families.BraydenMesserschmidt;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Online.API;
using GentrysQuest.Game.Overlays.LoginOverlay;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class MainMenu : Screen
    {
        private Box background;
        private TitleText title;
        private DrawableTrack menuTheme;
        private MainMenuButton playButton;
        private MainMenuButton quitButton;
        private LoginOverlay loginOverlay;

        [BackgroundDependencyLoader]
        private void load(ITrackStore trackStore)
        {
            menuTheme = new DrawableTrack(trackStore.Get("Gentrys_Quest_Ambient_1.mp3"));
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                },
                title = new TitleText("Gentry's Quest")
                {
                    Alpha = 0
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 100,
                    Spacing = new Vector2(0, 50),
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        playButton = new MainMenuButton("Play")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        },
                        quitButton = new MainMenuButton("Quit")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        }
                    }
                },
                new Container
                {
                    Size = new Vector2(400),
                    Y = 35,
                    X = -100,
                    Origin = Anchor.CentreRight,
                    Anchor = Anchor.CentreRight,
                    Masking = true,
                    CornerExponent = 2,
                    CornerRadius = 15,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Color4(20, 20, 20, 200)
                        },
                        new Container
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.95f),
                            Child =
                                loginOverlay = new LoginOverlay()
                        }
                    }
                }
            };
            playButton.SetAction(delegate
            {
                Character character = new BraydenMesserschmidt();
                Weapon weapon = new BraydensOsuPen();
                character.Experience.Level.Current.Value = 100;
                character.UpdateStats();
                weapon.Experience.Level.Current.Value = 100;
                weapon.UpdateStats();
                character.Weapon = weapon;
                GameData.Add(character);
                GameData.EquipCharacter(character);
                character.Artifacts.Equip(new MadokaChibiPlush(), 0);
                character.Artifacts.Equip(new MadokaChibiPlush(), 1);
                character.Artifacts.Equip(new MadokaChibiPlush(), 2);
                character.Artifacts.Equip(new MadokaChibiPlush(), 3);
                character.Artifacts.Equip(new MadokaChibiPlush(), 4);
                this.Push(new Gameplay.Gameplay());
            });
            quitButton.SetAction(delegate
            {
                _ = APIAccess.DeleteToken();
                Game.Exit();
            });
        }

        public void press_play()
        {
            title.FadeOut(200);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            background.FadeColour(ColourInfo.GradientVertical(
                new Colour4(72, 72, 72, 255),
                new Colour4(58, 58, 58, 255)
            ), 500);
            title.Delay(120).Then().FadeIn(120);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AudioManager.ChangeMusic(menuTheme);
        }
    }
}
