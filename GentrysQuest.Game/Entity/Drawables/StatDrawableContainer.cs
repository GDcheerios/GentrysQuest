using System.Linq;
using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class StatDrawableContainer : CompositeDrawable
    {
        private FillFlowContainer<StatDrawable> statDrawables;

        private const float HEIGHT = 20;

        public StatDrawableContainer()
        {
            Masking = true;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new SpriteText
                {
                    Text = "Stats",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    Font = FontUsage.Default.With(size: 36)
                },
                new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1, 0.8f),
                    Position = new Vector2(0, 32),
                    ClampExtension = 1,
                    ScrollbarVisible = false,
                    Child = statDrawables = new FillFlowContainer<StatDrawable>
                    {
                        Position = new Vector2(0, 0),
                        Direction = FillDirection.Vertical,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = HEIGHT,
                    Children =
                    [
                        new GqText("Stat")
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Margin = new MarginPadding { Left = 10 }
                        },
                        new GqText("+")
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        new GqText("Total")
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding { Right = 10 }
                        },
                    ]
                }
            };
        }

        public void AddStat(StatDrawable statDrawable, bool isNew = false)
        {
            statDrawables.Add(statDrawable);
            if (isNew) statDrawable.NewDisplay();
        }

        public StatDrawable GetStatDrawable(string identifier) => statDrawables.Children.FirstOrDefault(statDrawable => statDrawable.Identifier == identifier);
        public StatDrawable[] GetStatDrawables() => statDrawables.Children.ToArray();

        public void Clear() => statDrawables.Clear();
    }
}
