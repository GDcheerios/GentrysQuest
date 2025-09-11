using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class StatDrawable : CompositeDrawable
    {
        private const int DURATION = 100;

        public string Identifier { get; private set; }
        public Bindable<double> Value { get; private set; } = new();
        public Bindable<double> AdditionalValue { get; private set; } = new();

        private bool isPercent;

        private Box backgroundBox;
        private Box newIndicationBox;
        private SpriteText nameText;
        private SpriteText additionalValueText;
        private SpriteText valueText;
        private SpriteText changedToValue;
        private SpriteIcon arrowIndicator;
        private SpriteText newIndicationText;
        private Container nameContainer;
        private Container valueContainer;
        private Container additionalValueContainer;

        private const float CORNER_RADIUS = 10;
        private Colour4 backgroundColour = Colour4.DarkGray;

        public StatDrawable(string name, [CanBeNull] string identifier = null)
        {
            Identifier = identifier ?? name;
            Name = name;
        }

        public StatDrawable(Stat stat)
        {
            isPercent = stat.IsPercent;
            Name = stat.Name;
            Identifier = stat.Name;
            Value.Value = stat.Total();
            AdditionalValue.Value = stat.GetAdditional();
        }

        public StatDrawable(Buff attribute, [CanBeNull] string identifier = null)
        {
            isPercent = attribute.IsPercent;
            var name = attribute.StatType.ToString();
            Identifier = identifier ?? name;
            Name = name;
            Value.Value = attribute.Value.Value;
            AdditionalValue.Value = attribute.Level;
        }

        private void setUpdateEvent()
        {
            string percentText = isPercent ? "%" : " ";
            Value.ValueChanged += _ => valueText.Text = Value.Value + percentText;
            AdditionalValue.ValueChanged += _ => additionalValueText.Text = AdditionalValue.Value.ToString();
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
                    CornerRadius = 10,
                    CornerExponent = 2,
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        nameContainer = new Container
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.X,
                            Width = 1,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour,
                                },
                                nameText = new SpriteText
                                {
                                    Text = Name,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                        additionalValueContainer = new Container
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.X,
                            X = 0.35f,
                            Width = 0,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour
                                },
                                additionalValueText = new SpriteText
                                {
                                    Text = "",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                        valueContainer = new Container
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            RelativePositionAxes = Axes.X,
                            X = 0.70f,
                            Width = 0,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = backgroundColour,
                                },
                                valueText = new SpriteText
                                {
                                    Text = "",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Font = FontUsage.Default.With(size: 20)
                                }
                            ],
                        },
                    ]
                }
            ];

            string percentText = isPercent ? "%" : " ";
            nameContainer.Delay(DURATION * 2).ResizeWidthTo(0.33f, DURATION);
            additionalValueContainer.Delay(DURATION * 3).Then().ResizeWidthTo(0.33f, DURATION).Then()
                                    .Finally(_ => additionalValueText.Text = AdditionalValue.Value.ToString());
            valueContainer.Delay(DURATION * 4).Then().ResizeWidthTo(0.33f, DURATION).Then()
                          .Finally(_ => valueText.Text = Value.Value + percentText);
            setUpdateEvent();
        }

        /// <summary>
        /// Displays a notification to indicate it's a new stat.
        /// </summary>
        public void NewDisplay()
        {
            newIndicationBox.FadeIn(100);
            newIndicationText.FadeIn(100);
        }
    }
}
