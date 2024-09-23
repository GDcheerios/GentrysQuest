using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Audio.Music;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Screens.Intro
{
    public partial class Intro : Screen
    {
        private Sprite logo;
        private TextFlowContainer framework;
        private ISong introSong;
        bool isBandits = false;
        private List<ITextPart> osuText = new List<ITextPart>();

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Black,
                    RelativePositionAxes = Axes.Both,
                },
                logo = new Sprite
                {
                    Texture = textures.Get(@"logo"),
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(500, 500)
                },
                framework = new TextFlowContainer
                {
                    Width = 680,
                    Height = 240,
                    Y = -200,
                    Alpha = 0,
                    TextAnchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                }
            };
        }

        private void setIntroSong()
        {
            ISong[] songs = new ISong[] { new GentrysThemeOG(), new GentrysThemeSecond() };
            introSong = songs[MathBase.RandomChoice(2)];
            isBandits = introSong.Name == "Gentrys Quest Theme OG";
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            setIntroSong();
            AudioManager.Instance.ChangeMusic(introSong);
            Logger.Log(introSong.TimingPoints.GetPoint("LogoStart").ToString());
            logo.Delay(introSong.TimingPoints.GetPoint("LogoStart")).Then()
                .FadeInFromZero(2600, Easing.InOutBounce);

            osuText.Add(framework.AddText("osu!", t =>
            {
                t.Font = t.Font.With(size: 60);
                t.Alpha = 0;
                t.Scale = new Vector2(0, 1);
            }));
            framework.AddText("Framework", t =>
            {
                t.Font = t.Font.With(size: 60);
                t.Colour = ColourInfo.GradientVertical(Color4.White, Color4.DarkGray);
            });

            framework.Delay(introSong.TimingPoints.GetPoint("FrameworkStart")).Then()
                     .FadeInFromZero(introSong.TimingPoints.GetPoint("FrameworkFade"), Easing.InExpo);

            Schedule(() => osuText.SelectMany(t => t.Drawables).ForEach(t =>
            {
                t.Delay(introSong.TimingPoints.GetPoint("OsuStart")).Then()
                 .FadeIn(300)
                 .ScaleTo(new Vector2(1, 1), 300, Easing.OutQuart);
                t.Colour = Color4.LightPink;
            }));

            this.Delay(introSong.TimingPoints.GetPoint("FadeOut"))
                .Then()
                .FadeOut(3000, Easing.Out)
                .Finally(_ =>
                {
                    this.Push(new MainMenu.MainMenu(isBandits));
                });
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            this.FadeIn(500, Easing.OutQuint);
        }
    }
}
