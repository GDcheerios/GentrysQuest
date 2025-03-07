using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Graphics.Dialogue
{
    public partial class AutoDialogueBox : Container
    {
        private readonly string displayText;
        private readonly string author;
        private readonly double duration;
        private DialogueTextFlowContainer textDisplay;
        private SpriteText authorDisplay;

        public AutoDialogueBox(string author, string displayText, double duration = 1000)
        {
            this.author = author;
            this.displayText = displayText;
            this.duration = duration;
            Masking = true;
            CornerRadius = 10;
            CornerExponent = 2;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            this.FadeIn(100);

            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.7f, 0.35f);
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            Y = -50;

            Children =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                            Alpha = 0.8f
                        },
                        new Container
                        {
                            Size = new Vector2(0.9f, 0.9f),
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Masking = true,
                            CornerRadius = 10,
                            CornerExponent = 2,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Colour = Colour4.Black,
                                    Alpha = 0.8f
                                },
                                authorDisplay = new SpriteText
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Text = author,
                                    Colour = Colour4.White,
                                    Font = FontUsage.Default.With(size: 30),
                                    Margin = new MarginPadding { Left = 10, Top = 10 },
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding(35),
                                    Child = textDisplay = new DialogueTextFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Colour = Colour4.White,
                                        Y = 20
                                    }
                                }
                            ]
                        }
                    ]
                }
            ];
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            double displayTime = textDisplay.AddTextWithEffect(displayText, duration);
        }
    }
}
