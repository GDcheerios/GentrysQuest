using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Entity.Drawables
{
    // TODO: Add way to display percentage
    public partial class StatDrawable : CompositeDrawable
    {
        private const int DURATION = 500;

        public string Identifier { get; private set; }
        public new string Name { get; private set; }

        private bool isPercent;
        private float width;
        private float height;

        private Box backgroundBox;
        private Box newIndicationBox;
        private SpriteText nameText;
        private SpriteText additionalValueText;
        private SpriteText valueText;
        private SpriteText changedToValue;
        private SpriteIcon arrowIndicator;
        private SpriteText newIndicationText;

        private const float CORNER_RADIUS = 10;
        private Colour4 backgroundColour = Colour4.DarkGray;

        public StatDrawable(string name, [CanBeNull] string identifier = null)
        {
            this.isPercent = isPercent;
            Identifier = identifier ?? name;
            Name = name;
            width = 0.33f;
        }

        public StatDrawable(Buff attribute, [CanBeNull] string identifier = null)
        {
            isPercent = attribute.IsPercent;
            var name = attribute.StatType.ToString();
            Identifier = identifier ?? name;
            Name = name;
            width = 0.495f;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 25;
            InternalChildren =
            [
                new Container
                {
                    Masking = true,
                    CornerRadius = CORNER_RADIUS,
                    CornerExponent = 2,
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = width,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour
                                },
                                nameText = new SpriteText
                                {
                                    Text = Name,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding { Left = 5 },
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = width,
                            Alpha = width == 0.495f ? 0 : 1,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour
                                },
                                additionalValueText = new SpriteText
                                {
                                    Text = "0",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding { Left = 5 },
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = width,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour
                                },
                                valueText = new SpriteText
                                {
                                    Text = "0",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding { Left = 5 },
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                    ]
                }
            ];
        }

        private string percentText => isPercent ? "%" : " ";

        /// <summary>
        /// Displays a notification to indicate it's a new stat.
        /// </summary>
        public void NewDisplay()
        {
            newIndicationBox.FadeIn(100);
            newIndicationText.FadeIn(100);
        }

        /// <summary>
        /// Updates the value displayed on the stat drawable.
        /// </summary>
        public void UpdateValue(float newValue)
        {
            if (valueText.Text == $"{newValue}{percentText}")
                return;

            animateValueChange(newValue);
        }

        private void animateValueChange(float newValue)
        {
            valueText.Text = newValue.ToString();
        }
    }
}
