using GentrysQuest.Game.Audio.Music;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace GentrysQuest.Game.Overlays
{
    public partial class AudioOverlay : CompositeDrawable
    {
        private TextFlowContainer innerText;
        private Container musicInfoContainer;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChild = musicInfoContainer = new Container
            {
                Scale = new Vector2(2.4f),
                X = 1000,
                Y = 20,
                AutoSizeAxes = Axes.X,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Height = 50,
                Masking = true,
                Shear = new Vector2(-0.2f, 0),
                Children =
                [
                    new Box
                    {
                        Colour = new Colour4(177, 177, 177, 177),
                        RelativeSizeAxes = Axes.Both,
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(1, 5),
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Colour = Colour4.Black,
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(1, 5),
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Colour = Colour4.Black,
                    },
                    innerText = new TextFlowContainer
                    {
                        Text = $" - ",
                        TextAnchor = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Shear = new Vector2(0.2f, 0),
                        Margin = new MarginPadding{Left = 100, Right = 100, Bottom = 10, Top = 10}
                    }
                ]
            };
        }

        public void DisplaySong(ISong song)
        {
            innerText.Text = $"{song.ArtistName} - {song.Name}";
            musicInfoContainer.FadeIn().Then().MoveToX(0, 300, Easing.In).Then().Delay(2000).Then().FadeOut(1000, Easing.In).Then().MoveToX(1000);
        }
    }
}
