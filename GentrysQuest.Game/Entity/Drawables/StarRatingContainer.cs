using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class StarRatingContainer : CompositeDrawable
    {
        public readonly Bindable<int> starRating = new Bindable<int>();
        private StarRatingDrawable starRatingDrawable1 = new StarRatingDrawable(1);
        private StarRatingDrawable starRatingDrawable2 = new StarRatingDrawable(2);
        private StarRatingDrawable starRatingDrawable3 = new StarRatingDrawable(3);
        private StarRatingDrawable starRatingDrawable4 = new StarRatingDrawable(4);
        private StarRatingDrawable starRatingDrawable5 = new StarRatingDrawable(5);

        private const int DELAY_INCREMENT = 60;

        public StarRatingContainer(int starRating)
        {
            this.starRating.Value = 1;
            this.starRating.BindValueChanged(setStarRating, true);
            this.starRating.Value = starRating;

            InternalChild = new FillFlowContainer()
            {
                Direction = FillDirection.Horizontal,
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Children = new[]
                {
                    starRatingDrawable1,
                    starRatingDrawable2,
                    starRatingDrawable3,
                    starRatingDrawable4,
                    starRatingDrawable5
                }
            };
        }

        private void setStarRating(ValueChangedEvent<int> valueChangedEvent)
        {
            int newVal = valueChangedEvent.NewValue;
            int oldVal = valueChangedEvent.OldValue;

            List<int> delayList = Enumerable.Range(0, 5).Select(i => (i + 1) * DELAY_INCREMENT).ToList();
            if (oldVal > newVal) delayList.Reverse();

            ColourInfo color = GetColorGradient(newVal);
            starRatingDrawable1.updateColour(color, newVal, delayList[0]);
            starRatingDrawable2.updateColour(color, newVal, delayList[1]);
            starRatingDrawable3.updateColour(color, newVal, delayList[2]);
            starRatingDrawable4.updateColour(color, newVal, delayList[3]);
            starRatingDrawable5.updateColour(color, newVal, delayList[4]);
        }

        public static ColourInfo GetColorGradient(int starRating)
        {
            switch (starRating)
            {
                case 1:
                    return ColourInfo.GradientVertical(Colour4.White, Colour4.LightGray);

                case 2:
                    return ColourInfo.GradientVertical(Colour4.Lime, Colour4.Green);

                case 3:
                    return ColourInfo.GradientVertical(Colour4.Aqua, Colour4.Blue);

                case 4:
                    return ColourInfo.GradientVertical(Colour4.DeepPink, Colour4.Purple);

                case 5:
                    return ColourInfo.GradientVertical(Colour4.Yellow, Colour4.DarkGoldenrod);
            }

            return ColourInfo.SingleColour(Colour4.Black);
        }

        public static Colour4 GetColor(int starRating)
        {
            switch (starRating)
            {
                case 1:
                    return Colour4.White;

                case 2:
                    return Colour4.LimeGreen;

                case 3:
                    return Colour4.Aqua;

                case 4:
                    return Colour4.DeepPink;

                case 5:
                    return Colour4.Gold;
            }

            return ColourInfo.SingleColour(Colour4.Black);
        }
    }
}
