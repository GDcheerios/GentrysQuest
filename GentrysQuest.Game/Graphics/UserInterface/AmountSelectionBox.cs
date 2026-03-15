using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Graphics.UserInterface
{
    public partial class AmountSelectionBox : Container
    {
        public string Prefix { get; init; }
        public string Suffix { get; init; }

        private Bindable<int> amount = new();
        private bool multiplyTen = false;
        private bool multiplyHundred = false;

        private MainGqButton decreaseButton;
        private MainGqButton increaseButton;
        private GqText amountText;

        /// <summary>
        /// Minimum value
        /// </summary>
        public int MinValue { get; set; } = 0;

        /// <summary>
        /// Maximum value
        /// </summary>
        public int MaxValue { get; set; } = int.MaxValue;

        /// <summary>
        /// Default value
        /// </summary>
        public int DefaultValue { get; set; } = 0;

        [BackgroundDependencyLoader]
        private void load()
        {
            Children =
            [
                decreaseButton = new MainGqButton("-")
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.2f, 60),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Action = decreaseAmount
                },
                increaseButton = new MainGqButton("+")
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.2f, 60),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Action = increaseAmount
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.5f, 75),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children =
                    [
                        amountText = new GqText($"{Prefix}{amount.Value}{Suffix}")
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = FontUsage.Default.With(size: 32),
                        },
                        new MainGqButton("Reset")
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(0.65f, 30),
                            Action = () => amount.Value = DefaultValue
                        }
                    ]
                }
            ];
            amount.BindValueChanged(_ => { amountText!.Text = $"{Prefix}{amount.Value}{Suffix}"; });
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                    multiplyTen = true;
                    break;

                case Key.ShiftLeft:
                    multiplyHundred = true;
                    break;
            }

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                    multiplyTen = false;
                    break;

                case Key.ShiftLeft:
                    multiplyHundred = false;
                    break;
            }

            base.OnKeyUp(e);
        }

        public int GetAmount() => amount.Value;

        public void SetAmount(int amount)
        {
            int potentialAmount = amount + this.amount.Value;
            potentialAmount = Math.Clamp(potentialAmount, MinValue, MaxValue);
            this.amount.Value = potentialAmount;
        }

        private void increaseAmount()
        {
            int amountToAdd = 1;
            if (multiplyTen) amountToAdd *= 10;
            if (multiplyHundred) amountToAdd *= 100;
            SetAmount(amountToAdd);
        }

        private void decreaseAmount()
        {
            int amountToRemove = 1;
            if (multiplyTen) amountToRemove *= 10;
            if (multiplyHundred) amountToRemove *= 100;
            SetAmount(-amountToRemove);
        }
    }
}
