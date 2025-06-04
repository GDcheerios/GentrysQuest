using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    // TODO: Add way to display percentage
    public partial class StatDrawable : CompositeDrawable
    {
        private const int DURATION = 500;

        public string Identifier { get; private set; }
        public new string Name { get; private set; }
        private bool isPercent;

        private Box backgroundBox;
        private Box newIndicationBox;
        private SpriteText nameText;
        private SpriteText valueText;
        private SpriteText changedToValue;
        private SpriteIcon arrowIndicator;
        private SpriteText newIndicationText;

        public StatDrawable(string name, float value, bool isMain, string identifier = null, bool isPercent = false)
        {
            this.isPercent = isPercent;
            Identifier = identifier ?? name;
            Name = name;

            setupDrawableProperties(isMain);
            InternalChildren = createChildren(value, isMain);
            initializeState();
        }

        public StatDrawable(Buff attribute, bool isMain, string identifier = null)
        {
            this.isPercent = attribute.IsPercent;
            var name = attribute.StatType.ToString();
            Identifier = identifier ?? name;
            Name = name;

            setupDrawableProperties(isMain);
            InternalChildren = createChildren((float)attribute.Value.Value, isMain);
            initializeState();
        }

        private void setupDrawableProperties(bool isMain)
        {
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(0.98f, isMain ? 100 : 50);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Margin = new MarginPadding { Top = 3 };
        }

        private Drawable[] createChildren(float value, bool isMain)
        {
            return new Drawable[]
            {
                backgroundBox = createBackgroundBox(),
                newIndicationBox = createNewIndicationBox(),
                newIndicationText = createNewIndicationText(),
                nameText = createNameText(),
                createStatContainer(value, isMain)
            };
        }

        private Box createBackgroundBox()
        {
            return new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = new Colour4(0, 0, 0, 180)
            };
        }

        private Box createNewIndicationBox()
        {
            return new Box
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Colour = Colour4.Gold,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1, 0.1f)
            };
        }

        private SpriteText createNewIndicationText()
        {
            return new SpriteText
            {
                Text = "new",
                Anchor = Anchor.Centre,
                Origin = Anchor.BottomCentre,
                Font = FontUsage.Default.With(size: 16),
                Colour = Colour4.Gold,
            };
        }

        private SpriteText createNameText()
        {
            return new SpriteText
            {
                Text = Name,
                Margin = new MarginPadding { Left = 5 },
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
            };
        }

        private FillFlowContainer createStatContainer(float value, bool isMain)
        {
            int fontSize = isMain ? 52 : 0;

            return new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                AutoSizeAxes = Axes.X,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Children = new Drawable[]
                {
                    changedToValue = createStatText(value, fontSize, true),
                    arrowIndicator = createArrowIndicator(),
                    valueText = createStatText(value, fontSize, false),
                }
            };
        }

        private SpriteText createStatText(float value, int fontSize, bool isChangedValue)
        {
            return new SpriteText
            {
                Text = $"{value}{percentText}",
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = isChangedValue
                    ? new MarginPadding { Left = 20, Right = 20 }
                    : new MarginPadding { Right = 20 },
                Font = fontSize > 0 ? FontUsage.Default.With(size: fontSize) : FontUsage.Default
            };
        }

        private SpriteIcon createArrowIndicator()
        {
            return new SpriteIcon
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Icon = FontAwesome.Solid.LongArrowAltRight,
                Colour = Colour4.Gray,
                Size = new Vector2(64)
            };
        }

        private void initializeState()
        {
            newIndicationBox.Hide();
            newIndicationText.Hide();
            arrowIndicator.ScaleTo(new Vector2(0, 1));
            changedToValue.ScaleTo(new Vector2(0, 1));
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
            valueText.Text = changedToValue.Text + percentText;
            backgroundBox.FlashColour(new Colour4(255, 255, 255, 100), DURATION);

            changedToValue.Text = $"{newValue}{percentText}";
            changedToValue.ScaleTo(1, DURATION * 0.2);
            arrowIndicator.ScaleTo(1, DURATION * 0.2);
            changedToValue.FadeColour(Colour4.Gold, DURATION * 0.2);

            valueText.FlashColour(Colour4.Gold, DURATION * 0.5);
            nameText.FlashColour(Colour4.Gold, DURATION * 0.5);
        }
    }
}
