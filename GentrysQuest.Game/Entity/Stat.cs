using System;
using GentrysQuest.Game.Utils;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Entity
{
    public class Stat
    {
        public readonly string Name; // display name for other languages and etc
        protected StatType StatType; // this is how we get what stat we're looking at
        protected readonly bool ResetsOnUpdate;

        /// <summary>
        /// This is the bonus stat calculation variable.
        /// Use this in entities UpdateStats method to determine its effect on the calculation.
        /// </summary>
        public int Point;

        public bool IsPercent { get; private set; } = false;

        public Bindable<double> Default { get; private set; } = new();
        public Bindable<double> Minimum { get; } = new();
        public Bindable<double> Current { get; private set; } = new();
        public Bindable<double> Additional { get; private set; } = new();

        public Stat(string name, StatType statType, double minimumValue, bool resetsOnUpdate = true)
        {
            Name = name;
            StatType = statType;
            Minimum.Value = minimumValue;
            SetDefaultValue(0);
            Current.Value = Total();
            ResetsOnUpdate = resetsOnUpdate;
            calculate();

            switch (statType)
            {
                case StatType.CritRate:
                    IsPercent = true;
                    break;

                case StatType.CritDamage:
                    IsPercent = true;
                    break;
            }
        }

        private void calculate()
        {
            double difference = Current.Value;
            Current.Value = Total();
            difference -= Current.Value;
            if (!ResetsOnUpdate) UpdateCurrentValue(difference);
        }

        public void ResetAdditionalValue() => Additional.Value = 0;

        public void RestoreValue() => Current.Value = Total();

        public void UpdateCurrentValue(double updateDifference)
        {
            var potentialChange = Current.Value + updateDifference;

            if (potentialChange > Total()) { Current.Value = Total(); }
            else if (potentialChange < 0) { Current.Value = 0; }
            else Current.Value = potentialChange;
        }

        public void SetDefaultValue(double value)
        {
            Default.Value = Minimum.Value + value;
            calculate();
        }

        public void Add(double value)
        {
            Additional.Value += value;
            calculate();
        }

        public virtual double GetDefault() => Math.Round(Default.Value, 2);
        public virtual double GetAdditional() => Math.Round(Additional.Value, 2);
        public virtual double GetCurrent() => Math.Round(Current.Value, 2);

        public void SetAdditional(double value)
        {
            Additional.Value = value;
            calculate();
        }

        public double GetPercentFromDefault(float percent) => MathBase.GetPercent(Default.Value, percent);
        public double GetPercentFromAdditional(float percent) => MathBase.GetPercent(Additional.Value, percent);
        public double GetPercentFromTotal(float percent) => MathBase.GetPercent(Total(), percent);

        public virtual double Total() => Math.Round(Default.Value + Additional.Value, 2);

        public override string ToString() => $"{Name}: {Default.Value} + {Additional.Value} ({Total()})";
    }
}
